using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AES128;

[MemoryDiagnoser]
public class TestFunctions
{
    Random rng = new Random();
    byte[] P = new byte[8000000];
    byte[] K = new byte[16];
    static bool ByteArrayEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }

    [Benchmark]
    public void RunEncryptionSync()
    {
        rng.NextBytes(P);
        rng.NextBytes(K);
        _ = aes128algorithm.AES128E(P, K);
    }

    [Benchmark]
    public void RunEncryptionParallel()
    {
        rng.NextBytes(P);
        rng.NextBytes(K);
        _ = aes128algorithm.AES128EP(P, K);
    }

    public static void BenchmarkAes128()
    {
        var _ = BenchmarkRunner.Run<TestFunctions>();
    }

    public static void TestAlgorithm()
    {
        var random = new Random();
        var PlainText = new byte[50000];
        random.NextBytes(PlainText);
        var Key = new byte[16];
        random.NextBytes(Key);
        var dataE = aes128algorithm.AES128E(PlainText, Key);

        var CipherText = dataE.Item1;
        var LastRoundKey = dataE.Item2;

        var dataD = aes128algorithm.AES128D(CipherText, LastRoundKey, 50000);

        var PlainTextAfterDecrypt = dataD.Item1;
        var FirstRoundKey = dataD.Item2;

        if (ByteArrayEqual(PlainText, PlainTextAfterDecrypt) && ByteArrayEqual(Key, FirstRoundKey))
        {
            Console.WriteLine("SUCCESS");
        }
        else
        {
            Console.WriteLine("FAIL");
        }
    }
}


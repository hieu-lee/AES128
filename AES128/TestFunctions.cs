using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

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

    static void PrintArray(byte[] arr)
    {
        var str = new string[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            str[i] = arr[i].ToString();
        }
        var s = "[";
        s += string.Join(", ", str);
        s += "]";
        Console.WriteLine(s);
    }

    public static void RunDemo()
    {
        Console.WriteLine("Type in text you want to encrypt:");
        var input = Console.ReadLine();
        var plaintext = Encoding.UTF8.GetBytes(input);
        var key = new byte[16];
        var random = new Random();
        random.NextBytes(key);
        Console.WriteLine("\nAuto-generated key:");
        PrintArray(key);
        var res = aes128algorithm.AES128Encrypt(plaintext, key);
        Console.WriteLine("\nAfter encryption:");
        PrintArray(res.Item1);
        Console.WriteLine("\n\nDo you want to decrypt? [Y/N]");
        var answer = Console.ReadLine();
        answer = answer.Trim();
        if (string.IsNullOrEmpty(answer) || answer.ToUpper() != "Y")
        {
            Console.WriteLine("\nDemo ended.");
        }
        else
        {
            var res_decrypt = aes128algorithm.AES128Decrypt(res.Item1, res.Item2, plaintext.Length);
            var after_decrypt = Encoding.UTF8.GetString(res_decrypt.Item1);
            Console.WriteLine("\nText after decrypt:");
            Console.WriteLine(after_decrypt);
        }
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


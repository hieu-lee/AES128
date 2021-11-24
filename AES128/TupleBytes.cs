namespace AES128;

public struct TupleBytes
{
    public byte[] Item1 { get; }
    public byte[] Item2 { get; }

    public TupleBytes(byte[] _Item1, byte[] _Item2)
    {
        Item1 = _Item1;
        Item2 = _Item2;
    }
}


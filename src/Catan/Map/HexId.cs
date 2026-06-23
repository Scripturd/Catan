namespace Catan.Map;

public readonly record struct HexId
{
    public int Value { get; }

    public HexId(int value)
    {
        Value = value;
    }
}
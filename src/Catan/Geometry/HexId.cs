namespace Catan.Geometry;

public readonly record struct HexId
{
    public int Value { get; }

    public HexId(int value)
    {
        Value = value;
    }
}
namespace Catan.Domain.Board;

public readonly record struct TileId
{
    public int Value { get; }

    public TileId(int value)
    {
        Value = value;
    }
}
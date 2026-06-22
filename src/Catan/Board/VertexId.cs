namespace Catan.Board;

public readonly record struct VertexId
{
    public int Value { get; }

    public VertexId(int value)
    {
        Value = value;
    }
}
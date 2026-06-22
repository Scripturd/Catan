namespace Catan.Domain.Board;

public readonly record struct EdgeId
{
    public int Value { get; }
    public EdgeId(int value) { Value = value; }
}
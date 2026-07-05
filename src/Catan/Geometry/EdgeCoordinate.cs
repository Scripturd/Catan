namespace Catan.Geometry;

public readonly record struct EdgeCoordinate
{
    public int Q { get; }
    public int R { get; }
    public EdgeDirection Direction { get; }

    public EdgeCoordinate(int q, int r, EdgeDirection direction)
    {
        Q = q;
        R = r;
        Direction = direction;
    }
}
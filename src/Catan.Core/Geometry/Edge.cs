namespace Catan.Geometry;

public readonly record struct Edge
{
    public int Q { get; }
    public int R { get; }
    public EdgeDirection Direction { get; }

    public Edge(int q, int r, EdgeDirection direction)
    {
        Q = q;
        R = r;
        Direction = direction;
    }
}
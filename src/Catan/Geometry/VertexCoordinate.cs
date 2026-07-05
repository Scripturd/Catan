namespace Catan.Geometry;

public readonly record struct VertexCoordinate
{
    public int Q { get; }
    public int R { get; }
    public VertexCorner Corner { get; }

    public VertexCoordinate(int q, int r, VertexCorner corner)
    {
        Q = q;
        R = r;
        Corner = corner;
    }
}
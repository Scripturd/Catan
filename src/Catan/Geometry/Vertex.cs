namespace Catan.Geometry;

public readonly record struct Vertex
{
    public int Q { get; }
    public int R { get; }
    public VertexCorner Corner { get; }

    public Vertex(int q, int r, VertexCorner corner)
    {
        Q = q;
        R = r;
        Corner = corner;
    }
}
namespace Catan.Geometry;

public sealed class Hex
{
    public HexId Id { get; }
    public IReadOnlyList<VertexId> Vertices { get; }
    public IReadOnlyList<EdgeId> Edges { get; }

    public Hex(
        HexId id,
        IReadOnlyList<VertexId> vertices,
        IReadOnlyList<EdgeId> edges)
    {
        Id = id;
        Vertices = vertices;
        Edges = edges;
    }
}
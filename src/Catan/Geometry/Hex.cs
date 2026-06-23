namespace Catan.Geometry;

public sealed class Hex
{
    public HexId Id { get; }
    public TerrainKind Terrain { get; }
    public IReadOnlyList<VertexId> Vertices { get; }
    public IReadOnlyList<EdgeId> Edges { get; }

    public Hex(
        HexId id,
        TerrainKind terrain,
        IReadOnlyList<VertexId> vertices,
        IReadOnlyList<EdgeId> edges)
    {
        Id = id;
        Terrain = terrain;
        Vertices = vertices;
        Edges = edges;
    }
}
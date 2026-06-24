namespace Catan.Map;

public sealed class Hex
{
    public HexId Id { get; }
    public int Q { get; }
    public int R { get; }
    public TerrainKind Terrain { get; }
    public IReadOnlyList<VertexId> Vertices { get; }
    public IReadOnlyList<EdgeId> Edges { get; }

    public Hex(
        HexId id,
        int q,
        int r,
        TerrainKind terrain,
        IReadOnlyList<VertexId> vertices,
        IReadOnlyList<EdgeId> edges)
    {
        Id = id;
        Q = q;
        R = r;
        Terrain = terrain;
        Vertices = vertices;
        Edges = edges;
    }
}
namespace Catan.Map;

public sealed class Hex
{
    public HexId Id { get; }
    public int Q { get; }
    public int R { get; }
    public TerrainType TerrainType { get; }
    public IReadOnlyList<VertexId> Vertices { get; }
    public IReadOnlyList<EdgeId> Edges { get; }

    public Hex(
        HexId id,
        int q,
        int r,
        TerrainType terrainType,
        IReadOnlyList<VertexId> vertices,
        IReadOnlyList<EdgeId> edges)
    {
        Id = id;
        Q = q;
        R = r;
        TerrainType = terrainType;
        Vertices = vertices;
        Edges = edges;
    }
}
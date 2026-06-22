namespace Catan.Domain.Board;

/// <summary>
/// A single hex tile: its terrain, its number token (null for the desert), and the
/// vertices/edges around it. Immutable; adjacency is held by ID.
/// </summary>
public sealed class Tile
{
    public TileId Id { get; }
    public TerrainType Terrain { get; }
    public int? NumberToken { get; }
    public IReadOnlyList<VertexId> Vertices { get; }
    public IReadOnlyList<EdgeId> Edges { get; }

    public Tile(
        TileId id,
        TerrainType terrain,
        int? numberToken,
        IReadOnlyList<VertexId> vertices,
        IReadOnlyList<EdgeId> edges)
    {
        Id = id;
        Terrain = terrain;
        NumberToken = numberToken;
        Vertices = vertices;
        Edges = edges;
    }
}

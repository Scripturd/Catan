namespace Catan.Domain.Board;

/// <summary>
/// A settlement/city spot where up to three tiles meet. Knows the tiles it touches
/// (for resource production), its incident edges, and its neighbour vertices (for the
/// placement distance rule). Immutable; adjacency is held by ID.
/// </summary>
public sealed class Vertex
{
    public VertexId Id { get; }
    public IReadOnlyList<TileId> Tiles { get; }
    public IReadOnlyList<EdgeId> Edges { get; }
    public IReadOnlyList<VertexId> AdjacentVertices { get; }

    public Vertex(
        VertexId id,
        IReadOnlyList<TileId> tiles,
        IReadOnlyList<EdgeId> edges,
        IReadOnlyList<VertexId> adjacentVertices)
    {
        Id = id;
        Tiles = tiles;
        Edges = edges;
        AdjacentVertices = adjacentVertices;
    }
}

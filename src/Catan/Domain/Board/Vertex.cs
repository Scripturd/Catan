namespace Catan.Domain.Board;

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
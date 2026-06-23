namespace Catan.Map;

public sealed class Vertex
{
    public VertexId Id { get; }
    public IReadOnlyList<HexId> Hexes { get; }
    public IReadOnlyList<EdgeId> Edges { get; }
    public IReadOnlyList<VertexId> AdjacentVertices { get; }

    public Vertex(
        VertexId id,
        IReadOnlyList<HexId> hexes,
        IReadOnlyList<EdgeId> edges,
        IReadOnlyList<VertexId> adjacentVertices)
    {
        Id = id;
        Hexes = hexes;
        Edges = edges;
        AdjacentVertices = adjacentVertices;
    }
}
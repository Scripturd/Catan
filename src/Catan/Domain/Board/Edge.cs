namespace Catan.Domain.Board;

public sealed class Edge
{
    public EdgeId Id { get; }
    public VertexId A { get; }
    public VertexId B { get; }
    public IReadOnlyList<TileId> Tiles { get; }

    public Edge(EdgeId id, VertexId a, VertexId b, IReadOnlyList<TileId> tiles)
    {
        Id = id;
        A = a;
        B = b;
        Tiles = tiles;
    }
}
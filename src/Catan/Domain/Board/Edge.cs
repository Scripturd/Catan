namespace Catan.Domain.Board;

/// <summary>
/// A road spot: the side between two vertices. Borders one or two tiles. Immutable;
/// adjacency is held by ID.
/// </summary>
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

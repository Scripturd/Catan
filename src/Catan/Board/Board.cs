namespace Catan;

public sealed class Board
{
    private readonly Dictionary<TileId, Tile> _tiles;
    private readonly Dictionary<VertexId, Vertex> _vertices;
    private readonly Dictionary<EdgeId, Edge> _edges;

    public Board(IReadOnlyList<Tile> tiles, IReadOnlyList<Vertex> vertices, IReadOnlyList<Edge> edges)
    {
        Tiles = tiles;
        Vertices = vertices;
        Edges = edges;
        _tiles = tiles.ToDictionary(t => t.Id);
        _vertices = vertices.ToDictionary(v => v.Id);
        _edges = edges.ToDictionary(e => e.Id);
    }

    public IReadOnlyList<Tile> Tiles { get; }
    public IReadOnlyList<Vertex> Vertices { get; }
    public IReadOnlyList<Edge> Edges { get; }

    public Tile GetTile(TileId id) => _tiles[id];
    public Vertex GetVertex(VertexId id) => _vertices[id];
    public Edge GetEdge(EdgeId id) => _edges[id];

    public IReadOnlyList<VertexId> AdjacentVertices(VertexId vertex) => _vertices[vertex].AdjacentVertices;
    public IReadOnlyList<EdgeId> EdgesOf(VertexId vertex) => _vertices[vertex].Edges;
    public IEnumerable<Tile> TilesWithNumber(int number) => Tiles.Where(t => t.NumberToken == number);
}
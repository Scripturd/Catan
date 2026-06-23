namespace Catan.Geometry;

public sealed class HexGrid
{
    private readonly Dictionary<HexId, Hex> _hexes;
    private readonly Dictionary<VertexId, Vertex> _vertices;
    private readonly Dictionary<EdgeId, Edge> _edges;

    public HexGrid(IReadOnlyList<Hex> hexes, IReadOnlyList<Vertex> vertices, IReadOnlyList<Edge> edges)
    {
        Hexes = hexes;
        Vertices = vertices;
        Edges = edges;
        _hexes = hexes.ToDictionary(t => t.Id);
        _vertices = vertices.ToDictionary(v => v.Id);
        _edges = edges.ToDictionary(e => e.Id);
    }

    public IReadOnlyList<Hex> Hexes { get; }
    public IReadOnlyList<Vertex> Vertices { get; }
    public IReadOnlyList<Edge> Edges { get; }

    public Hex GetHex(HexId id) => _hexes[id];
    public Vertex GetVertex(VertexId id) => _vertices[id];
    public Edge GetEdge(EdgeId id) => _edges[id];

    public IReadOnlyList<VertexId> AdjacentVertices(VertexId vertex) => _vertices[vertex].AdjacentVertices;
    public IReadOnlyList<EdgeId> EdgesOf(VertexId vertex) => _vertices[vertex].Edges;
}
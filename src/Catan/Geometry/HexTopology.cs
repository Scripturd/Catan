namespace Catan.Geometry;

internal sealed class HexTopology
{
    private readonly HashSet<Hex> _hexes = [];
    private readonly HashSet<Vertex> _vertices = [];
    private readonly HashSet<Edge> _edges = [];

    public IReadOnlyList<Hex> Hexes => [.. _hexes];
    public IReadOnlyList<Vertex> Vertices => [.. _vertices];
    public IReadOnlyList<Edge> Edges => [.. _edges];

    public void AddHex(Hex hex)
    {
        _hexes.Add(hex);

        foreach (var vertex in HexGeometry.VerticesOf(hex))
            _vertices.Add(vertex);

        foreach (var edge in HexGeometry.EdgesOf(hex))
            _edges.Add(edge);
    }

    public void Clear()
    {
        _hexes.Clear();
        _vertices.Clear();
        _edges.Clear();
    }

    public bool HasHex(Hex hex) => _hexes.Contains(hex);
    public bool HasVertex(Vertex vertex) => _vertices.Contains(vertex);
    public bool HasEdge(Edge edge) => _edges.Contains(edge);

    public IReadOnlyList<Hex> HexesAround(Vertex vertex) =>
        [.. HexGeometry.HexesAround(vertex).Where(HasHex)];

    public IReadOnlyList<Edge> EdgesAround(Vertex vertex) =>
        [.. HexGeometry.EdgesAround(vertex).Where(HasEdge)];

    public IReadOnlyList<Vertex> AdjacentVertices(Vertex vertex) =>
        [.. EdgesAround(vertex).Select(edge => HexGeometry.Opposite(edge, vertex))];
}
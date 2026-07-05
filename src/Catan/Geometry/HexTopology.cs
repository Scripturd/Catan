namespace Catan.Geometry;

internal sealed class HexTopology
{
    private readonly HashSet<HexCoordinate> _hexes = [];
    private readonly HashSet<VertexCoordinate> _vertices = [];
    private readonly HashSet<EdgeCoordinate> _edges = [];

    public IReadOnlyList<HexCoordinate> Hexes => [.. _hexes];
    public IReadOnlyList<VertexCoordinate> Vertices => [.. _vertices];
    public IReadOnlyList<EdgeCoordinate> Edges => [.. _edges];

    public void AddHex(HexCoordinate hex)
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

    public bool HasHex(HexCoordinate hex) => _hexes.Contains(hex);
    public bool HasVertex(VertexCoordinate vertex) => _vertices.Contains(vertex);
    public bool HasEdge(EdgeCoordinate edge) => _edges.Contains(edge);

    public IReadOnlyList<HexCoordinate> HexesAround(VertexCoordinate vertex) =>
        [.. HexGeometry.HexesAround(vertex).Where(HasHex)];

    public IReadOnlyList<EdgeCoordinate> EdgesAround(VertexCoordinate vertex) =>
        [.. HexGeometry.EdgesAround(vertex).Where(HasEdge)];

    public IReadOnlyList<VertexCoordinate> AdjacentVertices(VertexCoordinate vertex) =>
        [.. EdgesAround(vertex).Select(edge => HexGeometry.Opposite(edge, vertex))];
}
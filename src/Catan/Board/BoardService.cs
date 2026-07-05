namespace Catan.Board;

public sealed class BoardService
{
    private readonly Dictionary<HexCoordinate, TerrainType> _hexes = [];
    private readonly HashSet<VertexCoordinate> _vertices = [];
    private readonly HashSet<EdgeCoordinate> _edges = [];

    public IReadOnlyList<HexCoordinate> Hexes => [.. _hexes.Keys];
    public IReadOnlyList<VertexCoordinate> Vertices => [.. _vertices];
    public IReadOnlyList<EdgeCoordinate> Edges => [.. _edges];

    public void AddHex(HexCoordinate hex, TerrainType terrainType)
    {
        _hexes[hex] = terrainType;

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

    public TerrainType TerrainAt(HexCoordinate hex) => _hexes[hex];

    public IReadOnlyList<VertexCoordinate> VerticesOf(HexCoordinate hex) => HexGeometry.VerticesOf(hex);
    public (VertexCoordinate A, VertexCoordinate B) EndpointsOf(EdgeCoordinate edge) => HexGeometry.EndpointsOf(edge);

    public IReadOnlyList<HexCoordinate> HexesAround(VertexCoordinate vertex) =>
        [.. HexGeometry.HexesAround(vertex).Where(_hexes.ContainsKey)];

    public IReadOnlyList<EdgeCoordinate> EdgesAround(VertexCoordinate vertex) =>
        [.. HexGeometry.EdgesAround(vertex).Where(_edges.Contains)];

    public IReadOnlyList<VertexCoordinate> AdjacentVertices(VertexCoordinate vertex) =>
        [.. EdgesAround(vertex).Select(edge => Opposite(edge, vertex))];

    public IEnumerable<HexCoordinate> HexesOf(TerrainType terrain) =>
        _hexes.Where(entry => entry.Value == terrain).Select(entry => entry.Key);

    private static VertexCoordinate Opposite(EdgeCoordinate edge, VertexCoordinate vertex)
    {
        var (a, b) = HexGeometry.EndpointsOf(edge);
        return a == vertex ? b : a;
    }
}
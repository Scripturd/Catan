namespace Catan.Board;

public sealed class BoardService
{
    private readonly HexTopology _topology = new();
    private readonly Dictionary<Hex, TerrainType> _terrainByHex = [];

    public IReadOnlyList<Hex> Hexes => _topology.Hexes;
    public IReadOnlyList<Vertex> Vertices => _topology.Vertices;
    public IReadOnlyList<Edge> Edges => _topology.Edges;

    public void AddHex(Hex hex, TerrainType terrainType)
    {
        _topology.AddHex(hex);
        _terrainByHex[hex] = terrainType;
    }
    public void Clear()
    {
        _topology.Clear();
        _terrainByHex.Clear();
    }

    public TerrainType TerrainAt(Hex hex) => _terrainByHex[hex];
    public IEnumerable<Hex> HexesOf(TerrainType terrain) =>
        _terrainByHex.Where(entry => entry.Value == terrain).Select(entry => entry.Key);

    public IReadOnlyList<Vertex> VerticesOf(Hex hex) => HexGeometry.VerticesOf(hex);
    public (Vertex A, Vertex B) EndpointsOf(Edge edge) => HexGeometry.EndpointsOf(edge);
    public IReadOnlyList<Hex> HexesOf(Edge edge) => HexGeometry.HexesOf(edge);
    public IReadOnlyList<Hex> HexesAround(Vertex vertex) => _topology.HexesAround(vertex);
    public IReadOnlyList<Edge> EdgesAround(Vertex vertex) => _topology.EdgesAround(vertex);
    public IReadOnlyList<Vertex> AdjacentVertices(Vertex vertex) => _topology.AdjacentVertices(vertex);

    public bool IsCoastal(Edge edge) => HexGeometry.HexesOf(edge).Count(IsLand) == 1;

    private bool IsLand(Hex hex) => _terrainByHex.TryGetValue(hex, out var terrain) && terrain.IsLand;
}
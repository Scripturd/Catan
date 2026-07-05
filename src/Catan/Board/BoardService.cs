namespace Catan.Board;

public sealed class BoardService
{
    private readonly HexTopology _topology = new();
    private readonly Dictionary<HexCoordinate, TerrainType> _terrains = [];

    public IReadOnlyList<HexCoordinate> Hexes => _topology.Hexes;
    public IReadOnlyList<VertexCoordinate> Vertices => _topology.Vertices;
    public IReadOnlyList<EdgeCoordinate> Edges => _topology.Edges;

    public void AddHex(HexCoordinate hex, TerrainType terrainType)
    {
        _topology.AddHex(hex);
        _terrains[hex] = terrainType;
    }

    public void Clear()
    {
        _topology.Clear();
        _terrains.Clear();
    }

    public TerrainType TerrainAt(HexCoordinate hex) => _terrains[hex];
    public IEnumerable<HexCoordinate> HexesOf(TerrainType terrain) =>
        _terrains.Where(entry => entry.Value == terrain).Select(entry => entry.Key);

    public static IReadOnlyList<VertexCoordinate> VerticesOf(HexCoordinate hex) => HexGeometry.VerticesOf(hex);
    public static (VertexCoordinate A, VertexCoordinate B) EndpointsOf(EdgeCoordinate edge) => HexGeometry.EndpointsOf(edge);
    public IReadOnlyList<HexCoordinate> HexesAround(VertexCoordinate vertex) => _topology.HexesAround(vertex);
    public IReadOnlyList<EdgeCoordinate> EdgesAround(VertexCoordinate vertex) => _topology.EdgesAround(vertex);
    public IReadOnlyList<VertexCoordinate> AdjacentVertices(VertexCoordinate vertex) => _topology.AdjacentVertices(vertex);
}
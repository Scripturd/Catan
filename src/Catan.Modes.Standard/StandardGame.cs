using Catan.Board;
using Catan.Economy;
using Catan.Game;
using Catan.Geometry;
using Catan.Players;

namespace Catan.Standard;

public class StandardGame : IGameMode
{
    private readonly IReadOnlyList<Hex> _hexes =
    [
        new(0, -2), new(1, -2), new(2, -2),
        new(-1, -1), new(0, -1), new(1, -1), new(2, -1),
        new(-2, 0), new(-1, 0), new(0, 0), new(1, 0), new(2, 0),
        new(-2, 1), new(-1, 1), new(0, 1), new(1, 1),
        new(-2, 2), new(-1, 2), new(0, 2),
    ];
    private readonly TerrainType[] _terrainTypes =
    [
        TerrainType.Desert,
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Hills, TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains, TerrainType.Mountains
    ];
    private readonly IReadOnlyDictionary<Edge, Harbour> _harbours = new Dictionary<Edge, Harbour>
    {
        {new(0, -3, EdgeDirection.SouthEast), new(3)},
        {new(2, -3, EdgeDirection.SouthWest), new(2, ResourceType.Wool)},
        {new(-2, -1, EdgeDirection.East), new(2, ResourceType.Ore)},
        {new(3, -2, EdgeDirection.SouthWest), new(3)},
        {new(2, 0, EdgeDirection.East), new(3)},
        {new(-3, 1, EdgeDirection.East), new(2, ResourceType.Grain)},
        {new(1, 1, EdgeDirection.SouthEast), new(2, ResourceType.Brick)},
        {new(-2, 2, EdgeDirection.SouthWest), new(3)},
        {new(-1, 2, EdgeDirection.SouthEast), new(2, ResourceType.Lumber)},
    };

    public string Name => "Standard Catan";
    public int MinPlayerCount => 3;
    public int MaxPlayerCount => 4;

    public void Start(GameServices services, IReadOnlyList<PlayerId> players)
    {
        AddLandHexes(services, _hexes, _terrainTypes);

        new NumberTokenSpiral(services.Board, services.Tokens, services.Shuffler).Place();

        foreach (var harbour in _harbours)
            services.Harbours.Place(harbour.Key, harbour.Value);

        var desert = services.Board.HexesOf(TerrainType.Desert).First();
        services.Robber.Place(desert);
    }

    private static void AddLandHexes(GameServices services, IReadOnlyList<Hex> hexes, IReadOnlyList<TerrainType> terrainTypes)
    {
        var shuffledTerrainTypes = services.Shuffler.Shuffle(terrainTypes);

        int terrainIndex = 0;
        foreach (var hex in hexes)
            services.Board.AddHex(hex, shuffledTerrainTypes[terrainIndex++]);
    }
}

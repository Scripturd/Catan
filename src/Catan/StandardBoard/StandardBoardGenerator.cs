using Catan.Economy;
using Catan.Pieces;

namespace Catan.StandardBoard;

public class StandardBoardGenerator
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


    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly HarbourService _harbourService;
    private readonly Robber _robber;
    private readonly Shuffler _shuffler;
    private readonly NumberTokenSpiral _numberTokenSpiral;

    public StandardBoardGenerator(
        BoardService boardService, 
        NumberTokenService numberTokenService,
        HarbourService harbourService,
        Robber robber,
        Shuffler shuffler)
    {
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _harbourService = harbourService;
        _robber = robber;
        _shuffler = shuffler;
        _numberTokenSpiral = new(_boardService, _numberTokenService, _shuffler);
    }

    public void Create()
    {
        _boardService.Clear();
        _numberTokenService.Clear();
        _harbourService.Clear();

        AddLandHexes(_hexes, _terrainTypes);

        _numberTokenSpiral.Place();

        foreach (var harbour in _harbours)
            _harbourService.Place(harbour.Key, harbour.Value);

        MoveRobber();
    }

    private void AddLandHexes(
        IReadOnlyList<Hex> hexes,
        IReadOnlyList<TerrainType> terrainTypes)
    {
        var shuffledTerrainTypes = _shuffler.Shuffle(terrainTypes);

        int terrainIndex = 0;
        foreach (var hex in hexes)
        {
            var terrainType = shuffledTerrainTypes[terrainIndex++];
            _boardService.AddHex(hex, terrainType);
        }
    }

    private void MoveRobber()
    {
        var desert = _boardService.HexesOf(TerrainType.Desert).First();
        _robber.MoveTo(desert);
    }
}
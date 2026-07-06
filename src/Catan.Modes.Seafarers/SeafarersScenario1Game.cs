using Catan.Board;
using Catan.Economy;
using Catan.Game;
using Catan.Geometry;
using Catan.Pieces;
using Catan.Players;

namespace Catan.SeafarersScenario1;

public class SeafarersScenario1Game : IGameMode
{
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly HarbourService _harbourService;
    private readonly Robber _robber;
    private readonly Pirate _pirate;
    private readonly Shuffler _shuffler;

    public int MinPlayerCount { get; } = 3;
    public int MaxPlayerCount { get; } = 4;

    public SeafarersScenario1Game(
        BoardService boardService,
        NumberTokenService numberTokenService,
        HarbourService harbourService,
        Robber robber,
        Pirate pirate,
        Shuffler shuffler)
    {
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _harbourService = harbourService;
        _robber = robber;
        _pirate = pirate;
        _shuffler = shuffler;
    }

    public void Start(IReadOnlyList<PlayerId> players)
    {
        ISeafarersScenario1Setup setup = players.Count == 3
                ? new SeafarersScenario1ThreePlayerSetup()
                : new SeafarersScenario1FourPlayerSetup();

        AddLandHexes(setup.MainHexes, setup.MainTerrainTypes, setup.MainTokens);
        AddLandHexes(setup.SmallHexes, setup.SmallTerrainTypes, setup.SmallTokens);
        AddSeaHexes(setup.SeaHexes);

        PlaceHarbours(setup);

        MoveRobber();
        MovePirate(setup);
    }

    private void AddLandHexes(
        IReadOnlyList<Hex> hexes,
        IReadOnlyList<TerrainType> terrainTypes,
        IReadOnlyList<int> tokens)
    {
        var shuffledTerrainTypes = _shuffler.Shuffle(terrainTypes);
        var shuffledTokens = _shuffler.Shuffle(tokens);

        int terrainIndex = 0;
        int tokenIndex = 0;
        foreach (var hex in hexes)
        {
            var terrainType = shuffledTerrainTypes[terrainIndex++];
            _boardService.AddHex(hex, terrainType);

            if (TerrainYields.For(terrainType) != Yield.Nothing)
                _numberTokenService.Place(hex, new NumberToken(shuffledTokens[tokenIndex++]));
        }
    }

    private void AddSeaHexes(IReadOnlyList<Hex> hexes)
    {
        foreach (var hex in hexes)
            _boardService.AddHex(hex, TerrainType.Sea);
    }

    private void PlaceHarbours(ISeafarersScenario1Setup setup)
    {
        var shuffledEdges = _shuffler.Shuffle(setup.HarbourEdges);
        var edges = SelectNonAdjacentEdges(shuffledEdges, setup.Harbours.Count);

        for (int i = 0; i < setup.Harbours.Count; i++)
            _harbourService.Place(edges[i], setup.Harbours[i]);
    }

    private IReadOnlyList<Edge> SelectNonAdjacentEdges(IReadOnlyList<Edge> candidates, int count)
    {
        var selected = new List<Edge>();
        var takenVertices = new HashSet<Vertex>();

        foreach (var edge in candidates)
        {
            var (a, b) = _boardService.EndpointsOf(edge);
            if (takenVertices.Contains(a) || takenVertices.Contains(b))
                continue;

            selected.Add(edge);
            takenVertices.Add(a);
            takenVertices.Add(b);

            if (selected.Count == count)
                return selected;
        }

        throw new InvalidOperationException(
            $"Could not select {count} non-adjacent edges; only {selected.Count} of {candidates.Count} candidates are mutually non-adjacent.");
    }

    private void MoveRobber()
    {
        var desert = _boardService.HexesOf(TerrainType.Desert).Cast<Hex?>().FirstOrDefault();
        _robber.Place(desert ?? _numberTokenService.HexesWith(12).First());
    }

    private void MovePirate(ISeafarersScenario1Setup setup)
    {
        _pirate.Place(setup.PirateHex);
    }
}

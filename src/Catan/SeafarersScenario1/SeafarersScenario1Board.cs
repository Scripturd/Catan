using Catan.Economy;
using Catan.Pieces;

namespace Catan.SeafarersScenario1;

public class SeafarersScenario1Board
{
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly HarbourService _harbourService;
    private readonly Robber _robber;
    private readonly Pirate _pirate;
    private readonly Shuffler _shuffler;

    public SeafarersScenario1Board(
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

    public void Create(ISeafarersScenario1Setup setup)
    {
        AddLandHexes(setup.MainHexes, setup.MainTerrainTypes, setup.MainTokens);
        AddLandHexes(setup.SmallHexes, setup.SmallTerrainTypes, setup.SmallTokens);
        AddSeaHexes(setup.SeaHexes);

        PlaceHarbours(setup);

        MoveRobber();
        MovePirate(setup);
    }
    public void Clear()
    {
        _boardService.Clear();
        _numberTokenService.Clear();
        _harbourService.Clear();
        _robber.Remove();
        _pirate.Remove();
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
        var edges = HexGeometry.SelectNonAdjacentEdges(shuffledEdges, setup.Harbours.Count);

        for (int i = 0; i < setup.Harbours.Count; i++)
            _harbourService.Place(edges[i], setup.Harbours[i]);
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
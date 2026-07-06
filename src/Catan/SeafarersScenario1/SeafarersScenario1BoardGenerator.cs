using Catan.Economy;

namespace Catan.SeafarersScenario1;

public class SeafarersScenario1BoardGenerator
{
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly Shuffler _shuffler;

    public SeafarersScenario1BoardGenerator(
        BoardService boardService, 
        NumberTokenService numberTokenService, 
        Shuffler shuffler)
    {
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _shuffler = shuffler;
    }

    public void Create(ISeafarersScenario1Setup setup)
    {
        _boardService.Clear();
        _numberTokenService.Clear();

        AddLandHexes(setup.MainHexes, setup.MainTerrainTypes, setup.MainTokens);
        AddLandHexes(setup.SmallHexes, setup.SmallTerrainTypes, setup.SmallTokens);
        AddSeaHexes(setup.SeaHexes);
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
}
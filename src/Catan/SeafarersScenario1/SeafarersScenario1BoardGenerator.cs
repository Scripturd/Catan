namespace Catan.SeafarersScenario1;

public class SeafarersScenario1BoardGenerator
{
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly Shuffler _shuffler;

    private enum Region
    {
        Main,
        Small,
        Sea
    }

    public SeafarersScenario1BoardGenerator(BoardService boardService, NumberTokenService numberTokenService, Shuffler shuffler)
    {
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _shuffler = shuffler;
    }

    public void Create(ISeafarersScenario1Setup setup)
    {
        _boardService.Clear();
        _numberTokenService.Clear();

        var mainTerrain = _shuffler.Shuffle(setup.MainTerrain);
        var smallTerrain = _shuffler.Shuffle(setup.SmallTerrain);

        var coords = new List<HexCoordinate>();
        var terrains = new List<TerrainType>();
        var regions = new List<Region>();

        for (int i = 0; i < setup.MainCoords.Count; i++)
        {
            coords.Add(setup.MainCoords[i]);
            terrains.Add(mainTerrain[i]);
            regions.Add(Region.Main);
        }

        for (int i = 0; i < setup.SmallCoords.Count; i++)
        {
            coords.Add(setup.SmallCoords[i]);
            terrains.Add(smallTerrain[i]);
            regions.Add(Region.Small);
        }

        for (int i = 0; i < setup.SeaCoords.Count; i++)
        {
            coords.Add(setup.SeaCoords[i]);
            terrains.Add(TerrainType.Sea);
            regions.Add(Region.Sea);
        }

        for (int i = 0; i < coords.Count; i++)
            _boardService.AddHex(coords[i], terrains[i]);

        PlaceTokens(coords, terrains, regions, _shuffler.Shuffle(setup.MainTokens), _shuffler.Shuffle(setup.SmallTokens));
    }

    private void PlaceTokens(
    List<HexCoordinate> coords,
    List<TerrainType> terrains,
    List<Region> regions,
    List<int> mainTokens,
    List<int> smallTokens)
    {
        int mainIndex = 0;
        int smallIndex = 0;
        for (int h = 0; h < coords.Count; h++)
        {
            if (terrains[h] == TerrainType.Sea || terrains[h] == TerrainType.Desert)
                continue;

            if (regions[h] == Region.Main)
                _numberTokenService.Place(coords[h], new NumberToken(mainTokens[mainIndex++]));
            else if (regions[h] == Region.Small)
                _numberTokenService.Place(coords[h], new NumberToken(smallTokens[smallIndex++]));
        }
    }
}
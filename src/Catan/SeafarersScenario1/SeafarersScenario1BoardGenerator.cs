using Catan.Economy;

namespace Catan.SeafarersScenario1;

public class SeafarersScenario1BoardGenerator
{
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly HarbourService _harbourService;
    private readonly Shuffler _shuffler;

    public SeafarersScenario1BoardGenerator(
        BoardService boardService, 
        NumberTokenService numberTokenService,
        HarbourService harbourService,
        Shuffler shuffler)
    {
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _harbourService = harbourService;
        _shuffler = shuffler;
    }

    public void Create(ISeafarersScenario1Setup setup)
    {
        _boardService.Clear();
        _numberTokenService.Clear();
        _harbourService.Clear();

        AddLandHexes(setup.MainHexes, setup.MainTerrainTypes, setup.MainTokens);
        AddLandHexes(setup.SmallHexes, setup.SmallTerrainTypes, setup.SmallTokens);
        AddSeaHexes(setup.SeaHexes);

        PlaceHarbours(setup);
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
            $"Could not place {count} harbours on non-adjacent edges; only {selected.Count} of {candidates.Count} candidate edges are mutually non-adjacent.");
    }
}
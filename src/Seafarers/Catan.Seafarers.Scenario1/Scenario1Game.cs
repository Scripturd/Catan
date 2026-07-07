using Catan.Board;
using Catan.Economy;
using Catan.Game;
using Catan.Geometry;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Seafarers.Scenario1;

public class Scenario1Game : IGameMode
{
    public string Name => "Seafarers: Heading for New Shores";
    public int MinPlayerCount => 3;
    public int MaxPlayerCount => 4;

    public void Start(GameServices services, IReadOnlyList<PlayerId> players)
    {
        IScenario1Setup setup = players.Count == 3
                ? new Scenario1ThreePlayerSetup()
                : new Scenario1FourPlayerSetup();

        AddLandHexes(services, setup.MainHexes, setup.MainTerrainTypes, setup.MainTokens);
        AddLandHexes(services, setup.SmallHexes, setup.SmallTerrainTypes, setup.SmallTokens);
        AddSeaHexes(services, setup.SeaHexes);

        PlaceHarbours(services, setup);

        MoveRobber(services);
    }

    private static void AddLandHexes(
        GameServices services,
        IReadOnlyList<Hex> hexes,
        IReadOnlyList<TerrainType> terrainTypes,
        IReadOnlyList<int> tokens)
    {
        var shuffledTerrainTypes = services.Shuffler.Shuffle(terrainTypes);
        var shuffledTokens = services.Shuffler.Shuffle(tokens);

        int terrainIndex = 0;
        int tokenIndex = 0;
        foreach (var hex in hexes)
        {
            var terrainType = shuffledTerrainTypes[terrainIndex++];
            services.Board.AddHex(hex, terrainType);

            if (terrainType.Yield != Yield.Nothing)
                services.Tokens.Place(hex, new NumberToken(shuffledTokens[tokenIndex++]));
        }
    }

    private static void AddSeaHexes(GameServices services, IReadOnlyList<Hex> hexes)
    {
        foreach (var hex in hexes)
            services.Board.AddHex(hex, SeafarersTerrain.Sea);
    }

    private static void PlaceHarbours(GameServices services, IScenario1Setup setup)
    {
        var shuffledEdges = services.Shuffler.Shuffle(setup.HarbourEdges);
        var edges = SelectNonAdjacentEdges(services, shuffledEdges, setup.Harbours.Count);

        for (int i = 0; i < setup.Harbours.Count; i++)
            services.Harbours.Place(edges[i], setup.Harbours[i]);
    }

    private static IReadOnlyList<Edge> SelectNonAdjacentEdges(GameServices services, IReadOnlyList<Edge> candidates, int count)
    {
        var selected = new List<Edge>();
        var takenVertices = new HashSet<Vertex>();

        foreach (var edge in candidates)
        {
            var (a, b) = services.Board.EndpointsOf(edge);
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

    private static void MoveRobber(GameServices services)
    {
        var desert = services.Board.HexesOf(StandardTerrain.Desert).Cast<Hex?>().FirstOrDefault();
        services.Robber.Place(desert ?? services.Tokens.HexesWith(12).First());
    }
}

namespace Catan;

internal static class SeafarersScenario1
{
    private const string ThreePlayerMap =
        "oooo\n" +
        "~~~oo\n" +
        "~MM~oo\n" +
        "~MMM~o~\n" +
        "MMMM~o\n" +
        "MMM~o\n" +
        "MM~o";

    private const string FourPlayerMap =
        "ooooo\n" +
        "~~~~oo\n" +
        "~MMM~oo\n" +
        "~MMMM~oo\n" +
        "MMMMM~o\n" +
        "MMMM~o\n" +
        "MMM~o";

    private static readonly TerrainType[] ThreePlayerMainLand =
    {
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains
    };

    private static readonly TerrainType[] ThreePlayerSmallLand =
    {
        TerrainType.Pasture,
        TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains,
        TerrainType.Gold, TerrainType.Gold
    };

    private static readonly int[] ThreePlayerMainTokens =
    {
        2, 3, 4, 5, 5, 6, 6, 8, 8, 9, 10, 10, 11, 11
    };

    private static readonly int[] ThreePlayerSmallTokens =
    {
        3, 4, 4, 5, 8, 9, 10, 12
    };

    private static readonly TerrainType[] FourPlayerMainLand =
    {
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains, TerrainType.Mountains,
        TerrainType.Desert
    };

    private static readonly TerrainType[] FourPlayerSmallLand =
    {
        TerrainType.Forest,
        TerrainType.Pasture,
        TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains,
        TerrainType.Gold, TerrainType.Gold
    };

    private static readonly int[] FourPlayerMainTokens =
    {
        2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12
    };

    private static readonly int[] FourPlayerSmallTokens =
    {
        2, 3, 4, 5, 6, 8, 9, 10, 11
    };

    public static (HexGrid Grid, NumberLayout Numbers) Create(int playerCount)
    {
        var setup = playerCount switch
        {
            3 => (Map: ThreePlayerMap, MainLand: ThreePlayerMainLand, SmallLand: ThreePlayerSmallLand, MainTokens: ThreePlayerMainTokens, SmallTokens: ThreePlayerSmallTokens),
            4 => (Map: FourPlayerMap, MainLand: FourPlayerMainLand, SmallLand: FourPlayerSmallLand, MainTokens: FourPlayerMainTokens, SmallTokens: FourPlayerSmallTokens),
            _ => throw new ArgumentOutOfRangeException(nameof(playerCount), playerCount, "Scenario 1 supports 3 or 4 players.")
        };

        var random = new Random();
        var (mainCoords, smallCoords, seaCoords) = Parse(setup.Map);

        var mainTerrain = Shuffle(setup.MainLand, random);
        var smallBag = setup.SmallLand.ToList();
        while (smallBag.Count < smallCoords.Count)
            smallBag.Add(TerrainType.Sea);
        var smallTerrain = Shuffle(smallBag, random);

        var coords = new List<(int Q, int R)>();
        var terrains = new List<TerrainType>();
        var regions = new List<Region>();
        Add(coords, terrains, regions, mainCoords, mainTerrain, Region.Main);
        Add(coords, terrains, regions, smallCoords, smallTerrain, Region.Small);
        Add(coords, terrains, regions, seaCoords, seaCoords.Select(_ => TerrainType.Sea).ToList(), Region.Sea);

        var grid = HexGridBuilder.Build(coords, terrains);
        var numbers = PlaceTokens(coords, terrains, regions, Shuffle(setup.MainTokens, random), Shuffle(setup.SmallTokens, random));

        return (grid, numbers);
    }

    private enum Region
    {
        Main,
        Small,
        Sea
    }

    private static (List<(int Q, int R)> Main, List<(int Q, int R)> Small, List<(int Q, int R)> Sea) Parse(string map)
    {
        var main = new List<(int Q, int R)>();
        var small = new List<(int Q, int R)>();
        var sea = new List<(int Q, int R)>();
        var rows = map.Split('\n');
        for (int r = 0; r < rows.Length; r++)
            for (int c = 0; c < rows[r].Length; c++)
                switch (rows[r][c])
                {
                    case 'M': main.Add((c - r, r)); break;
                    case 'o': small.Add((c - r, r)); break;
                    case '~': sea.Add((c - r, r)); break;
                }

        return (main, small, sea);
    }

    private static void Add(
        List<(int Q, int R)> coords,
        List<TerrainType> terrains,
        List<Region> regions,
        IReadOnlyList<(int Q, int R)> from,
        IReadOnlyList<TerrainType> withTerrain,
        Region region)
    {
        for (int i = 0; i < from.Count; i++)
        {
            coords.Add(from[i]);
            terrains.Add(withTerrain[i]);
            regions.Add(region);
        }
    }

    private static NumberLayout PlaceTokens(
        IReadOnlyList<(int Q, int R)> coords,
        IReadOnlyList<TerrainType> terrains,
        IReadOnlyList<Region> regions,
        IReadOnlyList<int> mainTokens,
        IReadOnlyList<int> smallTokens)
    {
        var tokens = new Dictionary<HexId, NumberToken>();
        int mainIndex = 0;
        int smallIndex = 0;
        for (int h = 0; h < coords.Count; h++)
        {
            if (terrains[h] == TerrainType.Sea || terrains[h] == TerrainType.Desert)
                continue;

            if (regions[h] == Region.Main)
                tokens[new HexId(h)] = new NumberToken(mainTokens[mainIndex++]);
            else if (regions[h] == Region.Small)
                tokens[new HexId(h)] = new NumberToken(smallTokens[smallIndex++]);
        }

        return new NumberLayout(tokens);
    }

    private static List<T> Shuffle<T>(IReadOnlyList<T> items, Random random)
    {
        var pool = items.ToList();
        var shuffled = new List<T>(pool.Count);
        while (pool.Count > 0)
        {
            var index = random.Next(0, pool.Count);
            shuffled.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return shuffled;
    }
}
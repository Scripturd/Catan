namespace Catan;

public static class StandardBoard
{
    private static readonly TerrainType[] TerrainTypes =
    {
        TerrainType.Desert,
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Hills, TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains, TerrainType.Mountains
    };

    public static (HexGrid Grid, NumberTokenLayout Numbers) Create()
    {
        var random = new Random();
        var coords = Coords();
        var terrains = Shuffle(TerrainTypes, random);
        var grid = HexGridBuilder.Build(coords, terrains);
        var numbers = NumberTokenSpiral.Place(grid.Hexes, random);
        return (grid, numbers);
    }

    private static List<(int Q, int R)> Coords()
    {
        var coords = new List<(int Q, int R)>();
        for (int q = -2; q <= 2; q++)
            for (int r = -2; r <= 2; r++)
                if (Math.Abs(q + r) <= 2)
                    coords.Add((q, r));

        return coords;
    }

    private static List<TerrainType> Shuffle(IReadOnlyList<TerrainType> terrains, Random random)
    {
        var pool = terrains.ToList();
        var shuffled = new List<TerrainType>(pool.Count);
        while (pool.Count > 0)
        {
            var index = random.Next(0, pool.Count);
            shuffled.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return shuffled;
    }
}
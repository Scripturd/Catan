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

    public static (HexGrid Grid, NumberLayout Numbers) Create()
    {
        var random = new Random();
        var terrains = Shuffle(TerrainTypes, random);
        var grid = HexGridBuilder.Build(terrains);
        var numbers = NumberTokenSpiral.Place(grid.Hexes, random);
        return (grid, numbers);
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
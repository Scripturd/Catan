namespace Catan.StandardBoard;

public class StandardBoardGenerator
{
    private readonly TerrainType[] TerrainTypes =
    {
        TerrainType.Desert,
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Hills, TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains, TerrainType.Mountains
    };

    private readonly Shuffler _shuffler;
    private readonly NumberTokenSpiral _numberTokenSpiral;

    public StandardBoardGenerator(Shuffler shuffler, NumberTokenSpiral numberTokenSpiral)
    {
        _shuffler = shuffler;
        _numberTokenSpiral = numberTokenSpiral;
    }

    public (BoardService Grid, NumberTokenService Numbers) Create()
    {
        var coords = Coords();
        var terrains = _shuffler.Shuffle(TerrainTypes);
        var grid = new BoardService();
        for (int i = 0; i < coords.Count; i++)
            grid.AddHex(coords[i], terrains[i]);

        var numbers = _numberTokenSpiral.Place(grid);
        return (grid, numbers);
    }

    private List<HexCoordinate> Coords()
    {
        var coords = new List<HexCoordinate>();
        for (int q = -2; q <= 2; q++)
            for (int r = -2; r <= 2; r++)
                if (Math.Abs(q + r) <= 2)
                    coords.Add(new HexCoordinate(q, r));

        return coords;
    }
}
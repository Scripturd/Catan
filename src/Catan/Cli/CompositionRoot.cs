namespace Catan.Cli;

public sealed class CompositionRoot
{
    public HexGrid Grid { get; }
    public TerrainLayout Terrain { get; }
    public NumberLayout Numbers { get; }

    public CompositionRoot()
    {
        var (grid, terrain, numbers) = StandardBoard.Create();
        Grid = grid;
        Terrain = terrain;
        Numbers = numbers;
    }
}
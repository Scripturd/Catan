namespace Catan.Cli;

public sealed class CompositionRoot
{
    public Board Board { get; }
    public TerrainLayout Terrain { get; }
    public NumberLayout Numbers { get; }

    public CompositionRoot()
    {
        var (board, terrain, numbers) = StandardBoard.Create();
        Board = board;
        Terrain = terrain;
        Numbers = numbers;
    }
}
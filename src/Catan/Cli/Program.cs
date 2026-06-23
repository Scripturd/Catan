namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var board = StandardBoard.Create();
        Console.WriteLine($"Catan board — {board.Hexes.Count} hexes, {board.Vertices.Count} vertices, {board.Edges.Count} edges");
    }
}
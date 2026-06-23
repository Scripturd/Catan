namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        Console.WriteLine($"Catan board — {root.Board.Hexes.Count} hexes, {root.Board.Vertices.Count} vertices, {root.Board.Edges.Count} edges");
        Console.WriteLine($"{root.Numbers.Count} number tokens, {root.Terrain.HexesOf(TerrainKind.Forest).Count()} forests");
    }
}
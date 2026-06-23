namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        Console.WriteLine($"Catan board — {root.Grid.Hexes.Count} hexes, {root.Grid.Vertices.Count} vertices, {root.Grid.Edges.Count} edges");
        Console.WriteLine($"{root.Numbers.Count} number tokens, {root.Terrain.HexesOf(TerrainKind.Forest).Count()} forests");
    }
}
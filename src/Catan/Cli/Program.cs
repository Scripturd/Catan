namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var playerCount = UI.AskUserForInt("How many players? (Seafarers Scenario 1 supports 3 or 4)", min: 3, max: 4);
        var (grid, numbers) = SeafarersScenario1.Create(playerCount);

        Console.WriteLine();
        Console.WriteLine($"Seafarers Scenario 1 - Heading for New Shores ({playerCount} players)");
        Console.WriteLine();
        Console.WriteLine(BoardRenderer.ToText(grid, numbers));
        Console.WriteLine();

        var land = grid.Hexes.Count(h => h.TerrainType != TerrainType.Sea);
        var sea = grid.Hexes.Count(h => h.TerrainType == TerrainType.Sea);
        var gold = grid.Hexes.Count(h => h.TerrainType == TerrainType.Gold);
        Console.WriteLine($"hexes {grid.Hexes.Count}: land {land}, sea {sea}, gold {gold}; number tokens {numbers.Count}");
    }
}
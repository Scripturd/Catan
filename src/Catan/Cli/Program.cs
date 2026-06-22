using Catan.Domain.Pieces;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("Catan (pure .NET) — building types");
        Console.WriteLine();

        foreach (var type in new[] { BuildingType.Settlement, BuildingType.City, BuildingType.Capital })
        {
            Console.WriteLine($"  {type.Name,-11} yields {type.Yield} per adjacent tile");
        }
    }
}

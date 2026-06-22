using Catan.Board;
using Catan.Game;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var settlements = new SettlementRegistry();
        settlements.Place(new VertexId(0), new Settlement(new PlayerId(0)));

        Console.WriteLine($"Catan (pure .NET) — {settlements.All.Count} settlement(s) placed");
    }
}
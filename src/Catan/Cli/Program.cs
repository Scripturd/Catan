using Catan.Pieces;
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        var player = new PlayerId(0);

        var hex = root.Grid.Hexes.First(h => root.Numbers.At(h.Id) is not null);
        var roll = root.Numbers.At(hex.Id)!.Value.Value;
        root.Settlements.Place(hex.Vertices[0], new Settlement(player));

        root.ProduceResources.Execute(roll);
        Console.WriteLine($"Rolled {roll}, robber on desert  -> player 0 lumber = {root.Resources.Of(player).Lumber}");

        root.Robber.MoveTo(hex.Id);
        root.ProduceResources.Execute(roll);
        Console.WriteLine($"Rolled {roll}, robber on hex {hex.Id.Value} -> player 0 lumber = {root.Resources.Of(player).Lumber}");
    }
}
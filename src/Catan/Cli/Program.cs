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
        var token = root.Numbers.At(hex.Id)!.Value;
        var roll = token.Value;

        root.Settlements.Place(hex.Vertices[0], new Settlement(player));
        root.ProduceResources.Execute(roll);

        var hand = root.Resources.Of(player);
        Console.WriteLine($"Hex {hex.Id.Value}: {root.Terrain.At(hex.Id)}, number {roll}");
        Console.WriteLine($"Player 0 after rolling {roll}: brick {hand.Brick}, lumber {hand.Lumber}, wool {hand.Wool}, grain {hand.Grain}, ore {hand.Ore}");
    }
}
using Catan.Economy;
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        var players = new[] { new PlayerId(0), new PlayerId(1) };
        var setup = root.NewSetupPhase(players);

        while (!setup.IsComplete)
        {
            var player = setup.Current;
            var vertex = PickSettlementSpot(root);
            var road = root.Grid.EdgesOf(vertex).First(e => !root.Roads.ExistsAt(e));
            setup.PlaceFor(vertex, road);
            Console.WriteLine($"player {player.Value} placed settlement v{vertex.Value} + road e{road.Value}");
        }

        Console.WriteLine();
        Console.WriteLine("starting hands (granted by each player's second settlement):");
        foreach (var player in players)
            Console.WriteLine($"  player {player.Value}: {Describe(root.Resources.Of(player))}");
    }

    private static VertexId PickSettlementSpot(CompositionRoot root)
    {
        foreach (var vertex in root.Grid.Vertices)
        {
            if (root.Settlements.ExistsAt(vertex.Id) || root.Cities.ExistsAt(vertex.Id))
                continue;

            if (root.Grid.AdjacentVertices(vertex.Id).Any(n => root.Settlements.ExistsAt(n) || root.Cities.ExistsAt(n)))
                continue;

            return vertex.Id;
        }

        throw new InvalidOperationException("no legal settlement spot left");
    }

    private static string Describe(ResourceBag hand)
    {
        var parts = Enum.GetValues<ResourceKind>()
            .Where(kind => hand[kind] > 0)
            .Select(kind => $"{hand[kind]} {kind}")
            .ToArray();
        return parts.Length == 0 ? "nothing" : string.Join(", ", parts);
    }
}
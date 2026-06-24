using Catan.Economy;
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();

        var playerAmount = UI.AskUserForInt("How many players do you want in this session?", min: 2, max: 4);
        PlayerId[] players = new PlayerId[playerAmount];
        for (int i = 0; i < playerAmount; i++)
        {
            players[i] = new PlayerId(i);
        }

        foreach (var hex in root.Grid.Hexes)
        {
            var terrainType = hex.TerrainType;
            var numberToken = root.Numbers.At(hex.Id);

            if (numberToken.HasValue)
                Console.WriteLine($"(q: {hex.Q}, r: {hex.R}): {terrainType} {numberToken.Value.Number}");            
            else
                Console.WriteLine($"(q: {hex.Q}, r: {hex.R}): {terrainType}");
        }

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
        var parts = Enum.GetValues<ResourceType>()
            .Where(kind => hand[kind] > 0)
            .Select(kind => $"{hand[kind]} {kind}")
            .ToArray();
        return parts.Length == 0 ? "nothing" : string.Join(", ", parts);
    }
}
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        var player = new PlayerId(0);

        var v0 = new VertexId(0);
        root.PlaceStartingSettlement.Execute(player, v0);

        var neighbour = root.Grid.AdjacentVertices(v0)[0];
        root.PlaceStartingSettlement.Execute(player, neighbour);

        var touching = root.Grid.EdgesOf(v0)[0];
        root.PlaceStartingRoad.Execute(player, touching);

        var detached = root.Grid.Edges.First(e => e.A != v0 && e.B != v0 && !root.Roads.ExistsAt(e.Id)).Id;
        root.PlaceStartingRoad.Execute(player, detached);

        Console.WriteLine($"starting settlement v{v0.Value} (free, no road needed): placed = {root.Settlements.ExistsAt(v0)}");
        Console.WriteLine($"starting settlement v{neighbour.Value} (adjacent): placed = {root.Settlements.ExistsAt(neighbour)}");
        Console.WriteLine($"starting road e{touching.Value} (touches settlement): placed = {root.Roads.ExistsAt(touching)}");
        Console.WriteLine($"starting road e{detached.Value} (no building): placed = {root.Roads.ExistsAt(detached)}");
    }
}
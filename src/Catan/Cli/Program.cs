using Catan.Economy;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        var player = new PlayerId(0);
        root.Resources.Give(player, new ResourceBag(brick: 5, lumber: 5, wool: 5, grain: 5));

        VertexId Other(EdgeId e, VertexId v)
        {
            var edge = root.Grid.GetEdge(e);
            return edge.A == v ? edge.B : edge.A;
        }

        root.BuildRoad.Execute(player, new EdgeId(0));
        Console.WriteLine($"road with no network: placed = {root.Roads.ExistsAt(new EdgeId(0))}");

        var start = new VertexId(0);
        root.Settlements.Place(start, new Settlement(player));

        var e1 = root.Grid.EdgesOf(start)[0];
        root.BuildRoad.Execute(player, e1);
        var mid = Other(e1, start);

        var adjToStart = root.Grid.AdjacentVertices(start);
        var e2 = root.Grid.EdgesOf(mid).First(e => e != e1 && Other(e, mid) != start && !adjToStart.Contains(Other(e, mid)));
        root.BuildRoad.Execute(player, e2);
        var reached = Other(e2, mid);

        root.BuildSettlement.Execute(player, reached);

        var disconnected = root.Grid.Vertices.First(v => !root.Grid.EdgesOf(v.Id).Any(e => root.Roads.ExistsAt(e))).Id;
        root.BuildSettlement.Execute(player, disconnected);

        Console.WriteLine($"road from settlement (e{e1.Value}): placed = {root.Roads.ExistsAt(e1)}");
        Console.WriteLine($"road extending network (e{e2.Value}): placed = {root.Roads.ExistsAt(e2)}");
        Console.WriteLine($"settlement reached via road (v{reached.Value}): placed = {root.Settlements.ExistsAt(reached)}");
        Console.WriteLine($"settlement with no road (v{disconnected.Value}): placed = {root.Settlements.ExistsAt(disconnected)}");
    }
}
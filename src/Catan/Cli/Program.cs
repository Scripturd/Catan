using Catan.Economy;
using Catan.Players;

namespace Catan.Cli;

internal static class Program
{
    private static void Main()
    {
        var root = new CompositionRoot();
        var player = new PlayerId(0);
        root.Resources.Give(player, new ResourceBag(brick: 3, lumber: 3, wool: 3, grain: 3));

        var a = new VertexId(0);
        var adjacent = root.Grid.AdjacentVertices(a);
        var neighbour = adjacent[0];
        var far = root.Grid.Vertices.First(v => v.Id != a && !adjacent.Contains(v.Id)).Id;

        root.BuildSettlement.Execute(player, a);
        root.BuildSettlement.Execute(player, neighbour);
        root.BuildSettlement.Execute(player, far);

        Console.WriteLine($"vertex {a.Value}: settlement = {root.Settlements.ExistsAt(a)}");
        Console.WriteLine($"vertex {neighbour.Value} (adjacent to {a.Value}): settlement = {root.Settlements.ExistsAt(neighbour)}");
        Console.WriteLine($"vertex {far.Value} (not adjacent): settlement = {root.Settlements.ExistsAt(far)}");
    }
}
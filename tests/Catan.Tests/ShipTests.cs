using Catan.Game;
using Catan.Geometry;
using Catan.Players;
using Catan.Seafarers;

namespace Catan.Tests;

public sealed class ShipTests
{
    [Fact]
    public void Building_a_ship_places_it_and_pays_the_cost()
    {
        var ships = new ShipRegistry();
        var resources = new ResourceRegistry();
        var player = new PlayerId(0);
        resources.Give(player, Ship.Cost);
        var edge = new Edge(0, 0, EdgeDirection.East);

        new BuildShipUseCase(new RoadRegistry(), ships, resources).Execute(player, edge);

        Assert.True(ships.ExistsAt(edge));
        Assert.Equal(player, ships.At(edge)!.Owner);
        Assert.Equal(0, resources.Of(player).Total);
    }

    [Fact]
    public void Building_a_ship_without_resources_does_nothing()
    {
        var ships = new ShipRegistry();
        var edge = new Edge(0, 0, EdgeDirection.East);

        new BuildShipUseCase(new RoadRegistry(), ships, new ResourceRegistry()).Execute(new PlayerId(0), edge);

        Assert.False(ships.ExistsAt(edge));
    }
}

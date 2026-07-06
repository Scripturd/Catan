using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildShipUseCase
{
    private readonly RoadRegistry _roadRegistry;
    private readonly ShipRegistry _shipRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildShipUseCase(
        RoadRegistry roadRegistry,
        ShipRegistry shipRegistry,
        ResourceRegistry resourceRegistry)
    {
        _roadRegistry = roadRegistry;
        _shipRegistry = shipRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, Edge edge)
    {
        if (_roadRegistry.ExistsAt(edge) || _shipRegistry.ExistsAt(edge))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Ship.Cost))
            return;

        _resourceRegistry.Take(playerId, Ship.Cost);

        var ship = new Ship(playerId);
        _shipRegistry.Place(edge, ship);
    }
}
using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildRoadUseCase
{
    private readonly RoadRegistry _roadRegistry;
    private readonly ShipRegistry _shipRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildRoadUseCase(
        RoadRegistry roadRegistry,
        ShipRegistry shipRegistry,
        ResourceRegistry resourceRegistry)
    {
        _roadRegistry = roadRegistry;
        _shipRegistry = shipRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, EdgeId edge)
    {
        if (_roadRegistry.ExistsAt(edge) || _shipRegistry.ExistsAt(edge))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Road.Cost))
            return;

        _resourceRegistry.Take(playerId, Road.Cost);

        var road = new Road(playerId);
        _roadRegistry.Place(edge, road);
    }
}
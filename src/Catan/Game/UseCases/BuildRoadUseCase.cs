using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildRoadUseCase
{
    private readonly PlacementRules _rules;
    private readonly RoadRegistry _roadRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildRoadUseCase(
        PlacementRules rules,
        RoadRegistry roadRegistry,
        ResourceRegistry resourceRegistry)
    {
        _rules = rules;
        _roadRegistry = roadRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, EdgeCoordinate edge)
    {
        if (!_rules.EdgeIsVacant(edge))
            return;

        if (!_rules.ConnectsToNetwork(playerId, edge))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Road.Cost))
            return;

        _resourceRegistry.Take(playerId, Road.Cost);
        _roadRegistry.Place(edge, new Road(playerId));
    }
}
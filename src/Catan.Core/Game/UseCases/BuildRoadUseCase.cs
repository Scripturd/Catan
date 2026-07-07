using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildRoadUseCase
{
    private readonly StandardPlacementRules _rules;
    private readonly RoadRegistry _roadRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildRoadUseCase(
        StandardPlacementRules rules,
        RoadRegistry roadRegistry,
        ResourceRegistry resourceRegistry)
    {
        _rules = rules;
        _roadRegistry = roadRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, Edge edge)
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
using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class PlaceStartingRoadUseCase
{
    private readonly PlacementRules _rules;
    private readonly RoadRegistry _roadRegistry;

    public PlaceStartingRoadUseCase(
        PlacementRules rules,
        RoadRegistry roadRegistry)
    {
        _rules = rules;
        _roadRegistry = roadRegistry;
    }

    public void Execute(PlayerId playerId, EdgeCoordinate edge)
    {
        if (!_rules.EdgeIsVacant(edge))
            return;

        if (!_rules.TouchesOwnBuilding(playerId, edge))
            return;

        _roadRegistry.Place(edge, new Road(playerId));
    }
}
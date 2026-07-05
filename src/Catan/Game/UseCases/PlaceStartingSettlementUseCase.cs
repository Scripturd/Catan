using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class PlaceStartingSettlementUseCase
{
    private readonly PlacementRules _rules;
    private readonly SettlementRegistry _settlementRegistry;

    public PlaceStartingSettlementUseCase(
        PlacementRules rules,
        SettlementRegistry settlementRegistry)
    {
        _rules = rules;
        _settlementRegistry = settlementRegistry;
    }

    public void Execute(PlayerId playerId, VertexCoordinate vertex)
    {
        if (!_rules.SatisfiesDistanceRule(vertex))
            return;

        _settlementRegistry.Place(vertex, new Settlement(playerId));
    }
}
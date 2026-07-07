using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class PlaceStartingSettlementUseCase
{
    private readonly StandardPlacementRules _rules;
    private readonly SettlementRegistry _settlementRegistry;

    public PlaceStartingSettlementUseCase(
        StandardPlacementRules rules,
        SettlementRegistry settlementRegistry)
    {
        _rules = rules;
        _settlementRegistry = settlementRegistry;
    }

    public void Execute(PlayerId playerId, Vertex vertex)
    {
        if (!_rules.SatisfiesDistanceRule(vertex))
            return;

        _settlementRegistry.Place(vertex, new Settlement(playerId));
    }
}
using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildSettlementUseCase
{
    private readonly PlacementRules _rules;
    private readonly SettlementRegistry _settlementRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildSettlementUseCase(
        PlacementRules rules,
        SettlementRegistry settlementRegistry,
        ResourceRegistry resourceRegistry)
    {
        _rules = rules;
        _settlementRegistry = settlementRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, VertexId vertex)
    {
        if (!_rules.SatisfiesDistanceRule(vertex))
            return;

        if (!_rules.TouchesOwnRoad(playerId, vertex))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Settlement.Cost))
            return;

        _resourceRegistry.Take(playerId, Settlement.Cost);
        _settlementRegistry.Place(vertex, new Settlement(playerId));
    }
}
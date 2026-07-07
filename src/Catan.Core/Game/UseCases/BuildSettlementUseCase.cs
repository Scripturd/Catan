using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildSettlementUseCase
{
    private readonly IPlacementRules _placementRules;
    private readonly IPlayerNotifier _playerNotifier;
    private readonly SettlementRegistry _settlementRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildSettlementUseCase(
        IPlacementRules placementRules,
        IPlayerNotifier playerNotifier,
        SettlementRegistry settlementRegistry,
        ResourceRegistry resourceRegistry)
    {
        _placementRules = placementRules;
        _playerNotifier = playerNotifier;
        _settlementRegistry = settlementRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, Vertex vertex)
    {
        if (!_placementRules.SatisfiesDistanceRule(vertex))
        {
            _playerNotifier.Warning("You cannot build a settlement here. Settlements must be at least two intersections apart.");
            return;
        }

        if (!_placementRules.TouchesOwnRoad(playerId, vertex))
        {
            _playerNotifier.Warning("You cannot build a settlement here. It must connect to one of your roads.");
            return;
        }

        if (!_resourceRegistry.CanAfford(playerId, Settlement.Cost))
        {
            _playerNotifier.Warning("You cannot build a settlement. You do not have enough resources.");
            return;
        }

        _resourceRegistry.Take(playerId, Settlement.Cost);
        _settlementRegistry.Place(vertex, new Settlement(playerId));
    }
}
using Catan.Game.UseCases;
using Catan.Players;

namespace Catan.Game;

public sealed class SetupPhase
{
    private readonly PlaceStartingSettlementUseCase _placeSettlement;
    private readonly PlaceStartingRoadUseCase _placeRoad;
    private readonly GrantStartingResourcesUseCase _grantResources;
    private readonly IReadOnlyList<PlayerId> _order;
    private int _turn;

    public SetupPhase(
        IReadOnlyList<PlayerId> players,
        PlaceStartingSettlementUseCase placeSettlement,
        PlaceStartingRoadUseCase placeRoad,
        GrantStartingResourcesUseCase grantResources)
    {
        _placeSettlement = placeSettlement;
        _placeRoad = placeRoad;
        _grantResources = grantResources;
        _order = SnakeOrder(players);
    }

    public bool IsComplete => _turn >= _order.Count;

    public PlayerId Current => _order[_turn];

    private bool IsSecondPlacement => _turn >= _order.Count / 2;

    public void PlaceFor(VertexCoordinate settlement, EdgeCoordinate road)
    {
        if (IsComplete)
            return;

        var player = _order[_turn];
        _placeSettlement.Execute(player, settlement);
        _placeRoad.Execute(player, road);

        if (IsSecondPlacement)
            _grantResources.Execute(player, settlement);

        _turn++;
    }

    private static IReadOnlyList<PlayerId> SnakeOrder(IReadOnlyList<PlayerId> players)
    {
        var order = new List<PlayerId>(players.Count * 2);
        order.AddRange(players);
        for (var i = players.Count - 1; i >= 0; i--)
            order.Add(players[i]);
        return order;
    }
}
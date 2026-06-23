using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildSettlementUseCase
{
    private readonly SettlementRegistry _settlementRegistry;
    private readonly CityRegistry _cityRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildSettlementUseCase(
        SettlementRegistry settlementRegistry,
        CityRegistry cityRegistry,
        ResourceRegistry resourceRegistry)
    {
        _settlementRegistry = settlementRegistry;
        _cityRegistry = cityRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, VertexId vertex)
    {
        if (_settlementRegistry.ExistsAt(vertex) || _cityRegistry.ExistsAt(vertex))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Settlement.Cost))
            return;

        _resourceRegistry.Take(playerId, Settlement.Cost);

        var settlement = new Settlement(playerId);
        _settlementRegistry.Place(vertex, settlement);
    }
}
using Catan.Board;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildCityUseCase
{
    private readonly SettlementRegistry _settlementRegistry;
    private readonly CityRegistry _cityRegistry;
    private readonly ResourceRegistry _resourceRegistry;

    public BuildCityUseCase(
        SettlementRegistry settlementRegistry,
        CityRegistry cityRegistry,
        ResourceRegistry resourceRegistry)
    {
        _settlementRegistry = settlementRegistry;
        _cityRegistry = cityRegistry;
        _resourceRegistry = resourceRegistry;
    }

    public void Execute(PlayerId playerId, VertexId vertexId)
    {
        if (!_settlementRegistry.ExistsAt(vertexId))
            return;

        if (!_resourceRegistry.CanAfford(playerId, City.Cost))
            return;

        _resourceRegistry.Take(playerId, Settlement.Cost);

        var city = new City(playerId);
        _cityRegistry.Place(vertexId, city);
    }
}
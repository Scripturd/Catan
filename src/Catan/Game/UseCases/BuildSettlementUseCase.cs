using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildSettlementUseCase
{
    private readonly SettlementRegistry _settlementRegistry;
    private readonly CityRegistry _cityRegistry;
    private readonly ResourceRegistry _resourceRegistry;
    private readonly RoadRegistry _roadRegistry;
    private readonly HexGrid _grid;

    public BuildSettlementUseCase(
        SettlementRegistry settlementRegistry,
        CityRegistry cityRegistry,
        ResourceRegistry resourceRegistry,
        RoadRegistry roadRegistry,
        HexGrid grid)
    {
        _settlementRegistry = settlementRegistry;
        _cityRegistry = cityRegistry;
        _resourceRegistry = resourceRegistry;
        _roadRegistry = roadRegistry;
        _grid = grid;
    }

    public void Execute(PlayerId playerId, VertexId vertex)
    {
        if (_settlementRegistry.ExistsAt(vertex) || _cityRegistry.ExistsAt(vertex))
            return;

        foreach (var neighbour in _grid.AdjacentVertices(vertex))
            if (_settlementRegistry.ExistsAt(neighbour) || _cityRegistry.ExistsAt(neighbour))
                return;

        if (!_grid.EdgesOf(vertex).Any(e => _roadRegistry.At(e)?.Owner == playerId))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Settlement.Cost))
            return;

        _resourceRegistry.Take(playerId, Settlement.Cost);
        _settlementRegistry.Place(vertex, new Settlement(playerId));
    }
}
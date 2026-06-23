using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class PlaceStartingSettlementUseCase
{
    private readonly SettlementRegistry _settlementRegistry;
    private readonly CityRegistry _cityRegistry;
    private readonly HexGrid _grid;

    public PlaceStartingSettlementUseCase(
        SettlementRegistry settlementRegistry,
        CityRegistry cityRegistry,
        HexGrid grid)
    {
        _settlementRegistry = settlementRegistry;
        _cityRegistry = cityRegistry;
        _grid = grid;
    }

    public void Execute(PlayerId playerId, VertexId vertex)
    {
        if (_settlementRegistry.ExistsAt(vertex) || _cityRegistry.ExistsAt(vertex))
            return;

        foreach (var neighbour in _grid.AdjacentVertices(vertex))
            if (_settlementRegistry.ExistsAt(neighbour) || _cityRegistry.ExistsAt(neighbour))
                return;

        _settlementRegistry.Place(vertex, new Settlement(playerId));
    }
}
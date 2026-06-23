using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class PlaceStartingRoadUseCase
{
    private readonly RoadRegistry _roadRegistry;
    private readonly ShipRegistry _shipRegistry;
    private readonly SettlementRegistry _settlementRegistry;
    private readonly CityRegistry _cityRegistry;
    private readonly HexGrid _grid;

    public PlaceStartingRoadUseCase(
        RoadRegistry roadRegistry,
        ShipRegistry shipRegistry,
        SettlementRegistry settlementRegistry,
        CityRegistry cityRegistry,
        HexGrid grid)
    {
        _roadRegistry = roadRegistry;
        _shipRegistry = shipRegistry;
        _settlementRegistry = settlementRegistry;
        _cityRegistry = cityRegistry;
        _grid = grid;
    }

    public void Execute(PlayerId playerId, EdgeId edge)
    {
        if (_roadRegistry.ExistsAt(edge) || _shipRegistry.ExistsAt(edge))
            return;

        var ends = _grid.GetEdge(edge);
        if (!TouchesOwnBuilding(playerId, ends.A) && !TouchesOwnBuilding(playerId, ends.B))
            return;

        _roadRegistry.Place(edge, new Road(playerId));
    }

    private bool TouchesOwnBuilding(PlayerId player, VertexId vertex) =>
        _settlementRegistry.At(vertex)?.Owner == player || _cityRegistry.At(vertex)?.Owner == player;
}
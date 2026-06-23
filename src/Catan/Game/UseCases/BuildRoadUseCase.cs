using Catan.Pieces;
using Catan.Players;

namespace Catan.Game.UseCases;

public class BuildRoadUseCase
{
    private readonly RoadRegistry _roadRegistry;
    private readonly ShipRegistry _shipRegistry;
    private readonly ResourceRegistry _resourceRegistry;
    private readonly SettlementRegistry _settlementRegistry;
    private readonly CityRegistry _cityRegistry;
    private readonly HexGrid _grid;

    public BuildRoadUseCase(
        RoadRegistry roadRegistry,
        ShipRegistry shipRegistry,
        ResourceRegistry resourceRegistry,
        SettlementRegistry settlementRegistry,
        CityRegistry cityRegistry,
        HexGrid grid)
    {
        _roadRegistry = roadRegistry;
        _shipRegistry = shipRegistry;
        _resourceRegistry = resourceRegistry;
        _settlementRegistry = settlementRegistry;
        _cityRegistry = cityRegistry;
        _grid = grid;
    }

    public void Execute(PlayerId playerId, EdgeId edge)
    {
        if (_roadRegistry.ExistsAt(edge) || _shipRegistry.ExistsAt(edge))
            return;

        if (!ConnectsToNetwork(playerId, _grid.GetEdge(edge)))
            return;

        if (!_resourceRegistry.CanAfford(playerId, Road.Cost))
            return;

        _resourceRegistry.Take(playerId, Road.Cost);
        _roadRegistry.Place(edge, new Road(playerId));
    }

    private bool ConnectsToNetwork(PlayerId player, Edge edge) =>
        HasPresence(player, edge.A) || HasPresence(player, edge.B);

    private bool HasPresence(PlayerId player, VertexId vertex) =>
        _settlementRegistry.At(vertex)?.Owner == player
        || _cityRegistry.At(vertex)?.Owner == player
        || _grid.EdgesOf(vertex).Any(e => _roadRegistry.At(e)?.Owner == player);
}
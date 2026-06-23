using Catan.Players;

namespace Catan.Game;

public sealed class PlacementRules
{
    private readonly SettlementRegistry _settlements;
    private readonly CityRegistry _cities;
    private readonly RoadRegistry _roads;
    private readonly ShipRegistry _ships;
    private readonly HexGrid _grid;

    public PlacementRules(
        SettlementRegistry settlements,
        CityRegistry cities,
        RoadRegistry roads,
        ShipRegistry ships,
        HexGrid grid)
    {
        _settlements = settlements;
        _cities = cities;
        _roads = roads;
        _ships = ships;
        _grid = grid;
    }

    public bool VertexIsVacant(VertexId vertex) =>
        !_settlements.ExistsAt(vertex) && !_cities.ExistsAt(vertex);

    public bool SatisfiesDistanceRule(VertexId vertex) =>
        VertexIsVacant(vertex) && _grid.AdjacentVertices(vertex).All(VertexIsVacant);

    public bool TouchesOwnRoad(PlayerId player, VertexId vertex) =>
        _grid.EdgesOf(vertex).Any(e => _roads.At(e)?.Owner == player);

    public bool EdgeIsVacant(EdgeId edge) =>
        !_roads.ExistsAt(edge) && !_ships.ExistsAt(edge);

    public bool TouchesOwnBuilding(PlayerId player, EdgeId edge)
    {
        var ends = _grid.GetEdge(edge);
        return OwnsBuildingAt(player, ends.A) || OwnsBuildingAt(player, ends.B);
    }

    public bool ConnectsToNetwork(PlayerId player, EdgeId edge)
    {
        var ends = _grid.GetEdge(edge);
        return HasPresence(player, ends.A) || HasPresence(player, ends.B);
    }

    private bool OwnsBuildingAt(PlayerId player, VertexId vertex) =>
        _settlements.At(vertex)?.Owner == player || _cities.At(vertex)?.Owner == player;

    private bool HasPresence(PlayerId player, VertexId vertex) =>
        OwnsBuildingAt(player, vertex) || TouchesOwnRoad(player, vertex);
}
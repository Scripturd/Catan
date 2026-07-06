using Catan.Players;

namespace Catan.Game;

public sealed class PlacementRules
{
    private readonly SettlementRegistry _settlements;
    private readonly CityRegistry _cities;
    private readonly RoadRegistry _roads;
    private readonly BoardService _grid;

    public PlacementRules(
        SettlementRegistry settlements,
        CityRegistry cities,
        RoadRegistry roads,
        BoardService grid)
    {
        _settlements = settlements;
        _cities = cities;
        _roads = roads;
        _grid = grid;
    }

    public bool VertexIsVacant(Vertex vertex) =>
        !_settlements.ExistsAt(vertex) && !_cities.ExistsAt(vertex);

    public bool SatisfiesDistanceRule(Vertex vertex) =>
        VertexIsVacant(vertex) && _grid.AdjacentVertices(vertex).All(VertexIsVacant);

    public bool TouchesOwnRoad(PlayerId player, Vertex vertex) =>
        _grid.EdgesAround(vertex).Any(e => _roads.At(e)?.Owner == player);

    public bool EdgeIsVacant(Edge edge) => !_roads.ExistsAt(edge);

    public bool TouchesOwnBuilding(PlayerId player, Edge edge)
    {
        var (a, b) = _grid.EndpointsOf(edge);
        return OwnsBuildingAt(player, a) || OwnsBuildingAt(player, b);
    }

    public bool ConnectsToNetwork(PlayerId player, Edge edge)
    {
        var (a, b) = _grid.EndpointsOf(edge);
        return HasPresence(player, a) || HasPresence(player, b);
    }

    private bool OwnsBuildingAt(PlayerId player, Vertex vertex) =>
        _settlements.At(vertex)?.Owner == player || _cities.At(vertex)?.Owner == player;

    private bool HasPresence(PlayerId player, Vertex vertex) =>
        OwnsBuildingAt(player, vertex) || TouchesOwnRoad(player, vertex);
}
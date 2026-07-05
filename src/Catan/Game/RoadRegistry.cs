using Catan.Pieces;

namespace Catan.Game;

public class RoadRegistry
{
    private readonly Dictionary<EdgeCoordinate, Road> _roads = new();

    public IReadOnlyDictionary<EdgeCoordinate, Road> All => _roads;

    public bool ExistsAt(EdgeCoordinate edge) => _roads.ContainsKey(edge);
    public Road? At(EdgeCoordinate edge) => _roads.GetValueOrDefault(edge);

    public void Place(EdgeCoordinate edge, Road road)
    {
        if (_roads.ContainsKey(edge))
            throw new InvalidOperationException($"A road already exists at edge {edge}.");

        _roads[edge] = road;
    }
}
using Catan.Pieces;

namespace Catan.Game;

public class RoadRegistry
{
    private readonly Dictionary<Edge, Road> _roads = new();

    public IReadOnlyDictionary<Edge, Road> All => _roads;

    public bool ExistsAt(Edge edge) => _roads.ContainsKey(edge);
    public Road? At(Edge edge) => _roads.GetValueOrDefault(edge);

    public void Place(Edge edge, Road road)
    {
        if (_roads.ContainsKey(edge))
            throw new InvalidOperationException($"A road already exists at edge {edge}.");

        _roads[edge] = road;
    }
}
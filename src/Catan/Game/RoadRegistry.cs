using Catan.Pieces;

namespace Catan.Game;

public class RoadRegistry
{
    private readonly Dictionary<EdgeId, Road> _roads = new();

    public IReadOnlyDictionary<EdgeId, Road> All => _roads;

    public bool ExistsAt(EdgeId edge) => _roads.ContainsKey(edge);
    public Road? At(EdgeId edge) => _roads.GetValueOrDefault(edge);

    public void Place(EdgeId edge, Road road)
    {
        if (_roads.ContainsKey(edge))
            throw new InvalidOperationException($"A road already exists at edge {edge.Value}.");

        _roads[edge] = road;
    }
}
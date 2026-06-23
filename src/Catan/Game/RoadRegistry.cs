using Catan.Board;
using Catan.Pieces;

namespace Catan.Game;

public class RoadRegistry
{
    private readonly Dictionary<VertexId, Road> _roads = new();

    public IReadOnlyDictionary<VertexId, Road> All => _roads;

    public Road? At(VertexId vertex) => _roads.GetValueOrDefault(vertex);

    public void Place(VertexId vertex, Road road)
    {
        if (_roads.ContainsKey(vertex))
            throw new InvalidOperationException($"A road already exists at edge {vertex.Value}.");

        _roads[vertex] = road;
    }
}
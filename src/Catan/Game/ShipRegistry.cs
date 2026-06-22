using Catan.Board;
using Catan.Pieces;

namespace Catan.Game;

internal class ShipRegistry
{
    private readonly Dictionary<VertexId, City> _cities = new();

    public IReadOnlyDictionary<VertexId, City> All => _cities;

    public City? At(VertexId vertex) => _cities.GetValueOrDefault(vertex);

    public void Place(VertexId vertex, City city)
    {
        if (_cities.ContainsKey(vertex))
            throw new InvalidOperationException($"Vertex {vertex.Value} is already occupied.");

        _cities[vertex] = city;
    }
}
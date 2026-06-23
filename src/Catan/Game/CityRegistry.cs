using Catan.Pieces;

namespace Catan.Game;

public sealed class CityRegistry
{
    private readonly Dictionary<VertexId, City> _cities = new();

    public IReadOnlyDictionary<VertexId, City> All => _cities;

    public bool ExistsAt(VertexId vertex) => _cities.ContainsKey(vertex);
    public City? At(VertexId vertex) => _cities.GetValueOrDefault(vertex);

    public void Place(VertexId vertex, City city)
    {
        if (_cities.ContainsKey(vertex))
            throw new InvalidOperationException($"A city already exists at vertex {vertex.Value}.");

        _cities[vertex] = city;
    }
}
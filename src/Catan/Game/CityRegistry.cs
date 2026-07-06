using Catan.Pieces;

namespace Catan.Game;

public sealed class CityRegistry
{
    private readonly Dictionary<Vertex, City> _cities = new();

    public IReadOnlyDictionary<Vertex, City> All => _cities;

    public bool ExistsAt(Vertex vertex) => _cities.ContainsKey(vertex);
    public City? At(Vertex vertex) => _cities.GetValueOrDefault(vertex);

    public void Place(Vertex vertex, City city)
    {
        if (_cities.ContainsKey(vertex))
            throw new InvalidOperationException($"A city already exists at vertex {vertex}.");

        _cities[vertex] = city;
    }
}
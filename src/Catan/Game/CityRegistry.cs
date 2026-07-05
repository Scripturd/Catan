using Catan.Pieces;

namespace Catan.Game;

public sealed class CityRegistry
{
    private readonly Dictionary<VertexCoordinate, City> _cities = new();

    public IReadOnlyDictionary<VertexCoordinate, City> All => _cities;

    public bool ExistsAt(VertexCoordinate vertex) => _cities.ContainsKey(vertex);
    public City? At(VertexCoordinate vertex) => _cities.GetValueOrDefault(vertex);

    public void Place(VertexCoordinate vertex, City city)
    {
        if (_cities.ContainsKey(vertex))
            throw new InvalidOperationException($"A city already exists at vertex {vertex}.");

        _cities[vertex] = city;
    }
}
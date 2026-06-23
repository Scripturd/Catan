using Catan.Board;
using Catan.Pieces;

namespace Catan.Game;

internal class ShipRegistry
{
    private readonly Dictionary<VertexId, Ship> _ships = new();

    public IReadOnlyDictionary<VertexId, Ship> All => _ships;

    public Ship? At(VertexId vertex) => _ships.GetValueOrDefault(vertex);

    public void Place(VertexId vertex, Ship ship)
    {
        if (_ships.ContainsKey(vertex))
            throw new InvalidOperationException($"A ship already exists at edge {vertex.Value}.");

        _ships[vertex] = ship;
    }
}
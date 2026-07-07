using Catan.Geometry;

namespace Catan.Seafarers;

public class ShipRegistry
{
    private readonly Dictionary<Edge, Ship> _ships = [];

    public IReadOnlyDictionary<Edge, Ship> All => _ships;

    public bool ExistsAt(Edge edge) => _ships.ContainsKey(edge);
    public Ship? At(Edge edge) => _ships.GetValueOrDefault(edge);

    public void Place(Edge edge, Ship ship)
    {
        if (_ships.ContainsKey(edge))
            throw new InvalidOperationException($"A ship already exists at edge {edge}.");

        _ships[edge] = ship;
    }
}

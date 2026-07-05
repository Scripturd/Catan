using Catan.Pieces;

namespace Catan.Game;

public class ShipRegistry
{
    private readonly Dictionary<EdgeCoordinate, Ship> _ships = new();

    public IReadOnlyDictionary<EdgeCoordinate, Ship> All => _ships;

    public bool ExistsAt(EdgeCoordinate edge) => _ships.ContainsKey(edge);
    public Ship? At(EdgeCoordinate edge) => _ships.GetValueOrDefault(edge);

    public void Place(EdgeCoordinate edge, Ship ship)
    {
        if (_ships.ContainsKey(edge))
            throw new InvalidOperationException($"A ship already exists at edge {edge}.");

        _ships[edge] = ship;
    }
}
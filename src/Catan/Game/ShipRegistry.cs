using Catan.Pieces;

namespace Catan.Game;

public class ShipRegistry
{
    private readonly Dictionary<EdgeId, Ship> _ships = new();

    public IReadOnlyDictionary<EdgeId, Ship> All => _ships;

    public bool ExistsAt(EdgeId edge) => _ships.ContainsKey(edge);
    public Ship? At(EdgeId edge) => _ships.GetValueOrDefault(edge);

    public void Place(EdgeId edge, Ship ship)
    {
        if (_ships.ContainsKey(edge))
            throw new InvalidOperationException($"A ship already exists at edge {edge.Value}.");

        _ships[edge] = ship;
    }
}
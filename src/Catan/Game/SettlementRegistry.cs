using Catan.Pieces;

namespace Catan.Game;

public sealed class SettlementRegistry
{
    private readonly Dictionary<Vertex, Settlement> _settlements = new();

    public IReadOnlyDictionary<Vertex, Settlement> All => _settlements;

    public bool ExistsAt(Vertex vertex) => _settlements.ContainsKey(vertex);
    public Settlement? At(Vertex vertex) => _settlements.GetValueOrDefault(vertex);

    public void Place(Vertex vertex, Settlement settlement)
    {
        if (_settlements.ContainsKey(vertex))
            throw new InvalidOperationException($"A settlement already exists at vertex {vertex}.");

        _settlements[vertex] = settlement;
    }

    public void Remove(Vertex vertex)
    {
        if (!_settlements.Remove(vertex))
            throw new InvalidOperationException($"No settlement at vertex {vertex}.");
    }
}
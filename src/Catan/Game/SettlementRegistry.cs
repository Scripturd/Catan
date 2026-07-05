using Catan.Pieces;

namespace Catan.Game;

public sealed class SettlementRegistry
{
    private readonly Dictionary<VertexCoordinate, Settlement> _settlements = new();

    public IReadOnlyDictionary<VertexCoordinate, Settlement> All => _settlements;

    public bool ExistsAt(VertexCoordinate vertex) => _settlements.ContainsKey(vertex);
    public Settlement? At(VertexCoordinate vertex) => _settlements.GetValueOrDefault(vertex);

    public void Place(VertexCoordinate vertex, Settlement settlement)
    {
        if (_settlements.ContainsKey(vertex))
            throw new InvalidOperationException($"A settlement already exists at vertex {vertex}.");

        _settlements[vertex] = settlement;
    }

    public void Remove(VertexCoordinate vertex)
    {
        if (!_settlements.Remove(vertex))
            throw new InvalidOperationException($"No settlement at vertex {vertex}.");
    }
}
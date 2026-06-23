using Catan.Board;
using Catan.Pieces;

namespace Catan.Game;

public sealed class SettlementRegistry
{
    private readonly Dictionary<VertexId, Settlement> _settlements = new();

    public IReadOnlyDictionary<VertexId, Settlement> All => _settlements;

    public bool ExistsAt(VertexId vertex) => _settlements.ContainsKey(vertex);
    public Settlement? At(VertexId vertex) => _settlements.GetValueOrDefault(vertex);

    public void Place(VertexId vertex, Settlement settlement)
    {
        if (_settlements.ContainsKey(vertex))
            throw new InvalidOperationException($"Vertex {vertex.Value} is already occupied.");

        _settlements[vertex] = settlement;
    }

    public void Remove(VertexId vertex)
    {
        if (!_settlements.Remove(vertex))
            throw new InvalidOperationException($"No settlement at vertex {vertex.Value}.");
    }
}
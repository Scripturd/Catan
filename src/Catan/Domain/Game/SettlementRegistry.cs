using Catan.Domain.Board;
using Catan.Domain.Pieces;

namespace Catan.Domain.Game;

public sealed class SettlementRegistry
{
    private readonly Dictionary<VertexId, Settlement> _settlements = new();

    public IReadOnlyDictionary<VertexId, Settlement> All => _settlements;

    public Settlement? At(VertexId vertex) => _settlements.GetValueOrDefault(vertex);

    public void Place(VertexId vertex, Settlement settlement)
    {
        if (_settlements.ContainsKey(vertex))
            throw new InvalidOperationException($"Vertex {vertex.Value} is already occupied.");

        _settlements[vertex] = settlement;
    }
}
using Catan.Domain.Board;
using Catan.Domain.Pieces;

namespace Catan.Domain.Game;

public sealed class GameState
{
    private readonly Dictionary<VertexId, Building> _buildings = new();

    public IReadOnlyDictionary<VertexId, Building> Buildings => _buildings;

    public Building? BuildingAt(VertexId vertex) => _buildings.GetValueOrDefault(vertex);

    public void PlaceBuilding(VertexId vertex, Building building)
    {
        if (_buildings.ContainsKey(vertex))
            throw new InvalidOperationException($"Vertex {vertex.Value} is already occupied.");

        _buildings[vertex] = building;
    }
}
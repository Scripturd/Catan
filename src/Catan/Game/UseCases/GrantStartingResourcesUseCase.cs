using Catan.Economy;
using Catan.Players;

namespace Catan.Game.UseCases;

public class GrantStartingResourcesUseCase
{
    private readonly HexGrid _grid;
    private readonly SettlementRegistry _settlements;
    private readonly ResourceRegistry _resources;

    public GrantStartingResourcesUseCase(
        HexGrid grid,
        SettlementRegistry settlements,
        ResourceRegistry resources)
    {
        _grid = grid;
        _settlements = settlements;
        _resources = resources;
    }

    public void Execute(PlayerId playerId, VertexId vertex)
    {
        if (_settlements.At(vertex)?.Owner != playerId)
            return;

        foreach (var hexId in _grid.GetVertex(vertex).Hexes)
        {
            var yield = TerrainYields.For(_grid.GetHex(hexId).TerrainType);
            if (yield.Type != YieldType.Resource)
                continue;

            _resources.Give(playerId, ResourceBag.Of(yield.Resource, 1));
        }
    }
}
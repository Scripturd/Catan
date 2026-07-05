using Catan.Economy;
using Catan.Players;

namespace Catan.Game.UseCases;

public class GrantStartingResourcesUseCase
{
    private readonly BoardService _grid;
    private readonly SettlementRegistry _settlements;
    private readonly ResourceRegistry _resources;

    public GrantStartingResourcesUseCase(
        BoardService grid,
        SettlementRegistry settlements,
        ResourceRegistry resources)
    {
        _grid = grid;
        _settlements = settlements;
        _resources = resources;
    }

    public void Execute(PlayerId playerId, VertexCoordinate vertex)
    {
        if (_settlements.At(vertex)?.Owner != playerId)
            return;

        foreach (var hex in _grid.HexesAround(vertex))
        {
            var yield = TerrainYields.For(_grid.TerrainAt(hex));
            if (yield.Type != YieldType.Resource)
                continue;

            _resources.Give(playerId, ResourceBag.Of(yield.Resource, 1));
        }
    }
}
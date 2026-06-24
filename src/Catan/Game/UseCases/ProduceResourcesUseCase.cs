using Catan.Economy;
using Catan.Pieces;

namespace Catan.Game.UseCases;

public class ProduceResourcesUseCase
{
    private readonly HexGrid _grid;
    private readonly NumberLayout _numbers;
    private readonly SettlementRegistry _settlements;
    private readonly CityRegistry _cities;
    private readonly ResourceRegistry _resources;
    private readonly Robber _robber;

    public ProduceResourcesUseCase(
        HexGrid grid,
        NumberLayout numbers,
        SettlementRegistry settlements,
        CityRegistry cities,
        ResourceRegistry resources,
        Robber robber)
    {
        _grid = grid;
        _numbers = numbers;
        _settlements = settlements;
        _cities = cities;
        _resources = resources;
        _robber = robber;
    }

    public void Execute(int roll)
    {
        foreach (var hexId in _numbers.HexesWith(roll))
        {
            if (hexId == _robber.Hex)
                continue;

            var hex = _grid.GetHex(hexId);
            var yield = TerrainYields.For(hex.TerrainType);
            if (yield.Type != YieldType.Resource)
                continue;

            foreach (var vertex in hex.Vertices)
            {
                var settlement = _settlements.At(vertex);
                if (settlement is not null)
                    _resources.Give(settlement.Owner, ResourceBag.Of(yield.Resource, 1));

                var city = _cities.At(vertex);
                if (city is not null)
                    _resources.Give(city.Owner, ResourceBag.Of(yield.Resource, 2));
            }
        }
    }
}
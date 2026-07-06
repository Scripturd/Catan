using Catan.Economy;
using Catan.Pieces;

namespace Catan.Game.UseCases;

public class ProduceResourcesUseCase
{
    private readonly BoardService _boardService;
    private readonly NumberTokenService _numbers;
    private readonly SettlementRegistry _settlements;
    private readonly CityRegistry _cities;
    private readonly ResourceRegistry _resources;
    private readonly Robber _robber;

    public ProduceResourcesUseCase(
        BoardService boardService,
        NumberTokenService numbers,
        SettlementRegistry settlements,
        CityRegistry cities,
        ResourceRegistry resources,
        Robber robber)
    {
        _boardService = boardService;
        _numbers = numbers;
        _settlements = settlements;
        _cities = cities;
        _resources = resources;
        _robber = robber;
    }

    public void Execute(int roll)
    {
        foreach (var hex in _numbers.HexesWith(roll))
        {
            if (hex == _robber.Hex)
                continue;

            var yield = TerrainYields.For(_boardService.TerrainAt(hex));
            if (yield.Type != YieldType.Resource)
                continue;

            foreach (var vertex in _boardService.VerticesOf(hex))
            {
                var settlement = _settlements.At(vertex);
                if (settlement is not null)
                    _resources.Give(settlement.Owner, ResourceBag.Of(yield.Resource!, 1));

                var city = _cities.At(vertex);
                if (city is not null)
                    _resources.Give(city.Owner, ResourceBag.Of(yield.Resource!, 2));
            }
        }
    }
}
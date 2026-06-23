using Catan.Game;
using Catan.Game.UseCases;
using Catan.Pieces;

namespace Catan.Cli;

public sealed class CompositionRoot
{
    public HexGrid Grid { get; }
    public TerrainLayout Terrain { get; }
    public NumberLayout Numbers { get; }

    public SettlementRegistry Settlements { get; }
    public CityRegistry Cities { get; }
    public RoadRegistry Roads { get; }
    public ShipRegistry Ships { get; }
    public ResourceRegistry Resources { get; }
    public Robber Robber { get; }

    public ProduceResourcesUseCase ProduceResources { get; }
    public BuildSettlementUseCase BuildSettlement { get; }
    public BuildRoadUseCase BuildRoad { get; }
    public PlaceStartingSettlementUseCase PlaceStartingSettlement { get; }
    public PlaceStartingRoadUseCase PlaceStartingRoad { get; }

    public CompositionRoot()
    {
        var (grid, terrain, numbers) = StandardBoard.Create();
        Grid = grid;
        Terrain = terrain;
        Numbers = numbers;

        Settlements = new SettlementRegistry();
        Cities = new CityRegistry();
        Roads = new RoadRegistry();
        Ships = new ShipRegistry();
        Resources = new ResourceRegistry();
        Robber = new Robber(terrain.HexesOf(TerrainKind.Desert).First());

        ProduceResources = new ProduceResourcesUseCase(grid, numbers, terrain, Settlements, Cities, Resources, Robber);
        BuildSettlement = new BuildSettlementUseCase(Settlements, Cities, Resources, Roads, Grid);
        BuildRoad = new BuildRoadUseCase(Roads, Ships, Resources, Settlements, Cities, Grid);
        PlaceStartingSettlement = new PlaceStartingSettlementUseCase(Settlements, Cities, Grid);
        PlaceStartingRoad = new PlaceStartingRoadUseCase(Roads, Ships, Settlements, Cities, Grid);
    }
}
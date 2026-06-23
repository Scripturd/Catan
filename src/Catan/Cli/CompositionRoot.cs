using Catan.Game;
using Catan.Game.UseCases;
using Catan.Pieces;
using Catan.Players;

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
    public PlacementRules PlacementRules { get; }

    public ProduceResourcesUseCase ProduceResources { get; }
    public BuildSettlementUseCase BuildSettlement { get; }
    public BuildRoadUseCase BuildRoad { get; }
    public PlaceStartingSettlementUseCase PlaceStartingSettlement { get; }
    public PlaceStartingRoadUseCase PlaceStartingRoad { get; }
    public GrantStartingResourcesUseCase GrantStartingResources { get; }

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
        PlacementRules = new PlacementRules(Settlements, Cities, Roads, Ships, Grid);

        ProduceResources = new ProduceResourcesUseCase(grid, numbers, terrain, Settlements, Cities, Resources, Robber);
        BuildSettlement = new BuildSettlementUseCase(PlacementRules, Settlements, Resources);
        BuildRoad = new BuildRoadUseCase(PlacementRules, Roads, Resources);
        PlaceStartingSettlement = new PlaceStartingSettlementUseCase(PlacementRules, Settlements);
        PlaceStartingRoad = new PlaceStartingRoadUseCase(PlacementRules, Roads);
        GrantStartingResources = new GrantStartingResourcesUseCase(Grid, Terrain, Settlements, Resources);
    }

    public SetupPhase NewSetupPhase(IReadOnlyList<PlayerId> players) =>
        new(players, PlaceStartingSettlement, PlaceStartingRoad, GrantStartingResources);
}
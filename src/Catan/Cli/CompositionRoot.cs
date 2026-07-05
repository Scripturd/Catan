using Catan.Game;
using Catan.Game.UseCases;
using Catan.Pieces;
using Catan.Players;
using Catan.SeafarersScenario1;
using Catan.StandardBoard;

namespace Catan.Cli;

public sealed class CompositionRoot
{
    public Random Random = new();
    public BoardService Grid { get; }
    public NumberTokenService Numbers { get; }
    public Shuffler Shuffler { get; }

    public NumberTokenSpiral NumberTokenSpiral { get; }
    public StandardBoardGenerator StandardBoardGenerator { get; }

    public SeafarersScenario1BoardGenerator SeafarersScenario1BoardGenerator { get; }

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
        Grid = ;
        Numbers = ;
        Shuffler = new Shuffler(Random);

        NumberTokenSpiral = new NumberTokenSpiral(Random);
        StandardBoardGenerator = new StandardBoardGenerator(Shuffler, NumberTokenSpiral);

        SeafarersScenario1BoardGenerator = new SeafarersScenario1BoardGenerator(Shuffler);

        Settlements = new SettlementRegistry();
        Cities = new CityRegistry();
        Roads = new RoadRegistry();
        Ships = new ShipRegistry();
        Resources = new ResourceRegistry();
        Robber = new Robber(grid.HexesOf(TerrainType.Desert).First());
        PlacementRules = new PlacementRules(Settlements, Cities, Roads, Ships, Grid);

        ProduceResources = new ProduceResourcesUseCase(grid, numbers, Settlements, Cities, Resources, Robber);
        BuildSettlement = new BuildSettlementUseCase(PlacementRules, Settlements, Resources);
        BuildRoad = new BuildRoadUseCase(PlacementRules, Roads, Resources);
        PlaceStartingSettlement = new PlaceStartingSettlementUseCase(PlacementRules, Settlements);
        PlaceStartingRoad = new PlaceStartingRoadUseCase(PlacementRules, Roads);
        GrantStartingResources = new GrantStartingResourcesUseCase(Grid, Settlements, Resources);
    }

    public SetupPhase NewSetupPhase(IReadOnlyList<PlayerId> players) =>
        new(players, PlaceStartingSettlement, PlaceStartingRoad, GrantStartingResources);
}
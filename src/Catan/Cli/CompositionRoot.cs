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
    public BoardService BoardService { get; }
    public NumberTokenService NumberTokenService { get; }
    public HarbourService HarbourService { get; }
    public Shuffler Shuffler { get; }

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
        BoardService = new();
        NumberTokenService = new(BoardService);
        HarbourService = new(BoardService);
        Shuffler = new Shuffler(Random);

        StandardBoardGenerator = new StandardBoardGenerator(BoardService, NumberTokenService, HarbourService, Shuffler);

        SeafarersScenario1BoardGenerator = new SeafarersScenario1BoardGenerator(BoardService, NumberTokenService, HarbourService, Shuffler);

        Settlements = new SettlementRegistry();
        Cities = new CityRegistry();
        Roads = new RoadRegistry();
        Ships = new ShipRegistry();
        Resources = new ResourceRegistry();
        Robber = new Robber();
        PlacementRules = new PlacementRules(Settlements, Cities, Roads, Ships, BoardService);

        ProduceResources = new ProduceResourcesUseCase(BoardService, NumberTokenService, Settlements, Cities, Resources, Robber);
        BuildSettlement = new BuildSettlementUseCase(PlacementRules, Settlements, Resources);
        BuildRoad = new BuildRoadUseCase(PlacementRules, Roads, Resources);
        PlaceStartingSettlement = new PlaceStartingSettlementUseCase(PlacementRules, Settlements);
        PlaceStartingRoad = new PlaceStartingRoadUseCase(PlacementRules, Roads);
        GrantStartingResources = new GrantStartingResourcesUseCase(BoardService, Settlements, Resources);
    }

    public SetupPhase NewSetupPhase(IReadOnlyList<PlayerId> players) =>
        new(players, PlaceStartingSettlement, PlaceStartingRoad, GrantStartingResources);
}
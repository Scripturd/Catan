using Catan.Game;
using Catan.Game.UseCases;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Cli;

public sealed class CompositionRoot
{
    public Random Random = new();
    public BoardService BoardService { get; }
    public NumberTokenService NumberTokenService { get; }
    public HarbourService HarbourService { get; }
    public Shuffler Shuffler { get; }

    public SettlementRegistry Settlements { get; }
    public CityRegistry Cities { get; }
    public RoadRegistry Roads { get; }
    public ShipRegistry Ships { get; }
    public ResourceRegistry Resources { get; }
    public Robber Robber { get; }
    public MarkerRegistry Markers { get; }
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

        Settlements = new SettlementRegistry();
        Cities = new CityRegistry();
        Roads = new RoadRegistry();
        Ships = new ShipRegistry();
        Resources = new ResourceRegistry();
        Robber = new Robber();
        Markers = new MarkerRegistry();

        Shuffler = new Shuffler(Random);

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
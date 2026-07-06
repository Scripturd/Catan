using Catan.Game.UseCases;
using Catan.Modding;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Game;

public sealed class GameSession
{
    public BoardService Board { get; }
    public NumberTokenService Tokens { get; }
    public HarbourService Harbours { get; }
    public Robber Robber { get; }
    public Pirate Pirate { get; }
    public SettlementRegistry Settlements { get; }
    public CityRegistry Cities { get; }
    public RoadRegistry Roads { get; }
    public ShipRegistry Ships { get; }
    public ResourceRegistry Resources { get; }
    public IReadOnlyList<PlayerId> Players { get; }

    private readonly PlacementRules _rules;
    private readonly SetupPhase _setup;

    public GameSession(BoardDefinition definition, IReadOnlyList<PlayerId> players, Random random)
        : this(
            (board, tokens, harbours, robber, pirate, shuffler) =>
                new DataDrivenGameMode(definition, board, tokens, harbours, robber, pirate, shuffler),
            players,
            random)
    {
    }

    public GameSession(GameModeFactory createMode, IReadOnlyList<PlayerId> players, Random random)
    {
        Players = players;
        Board = new BoardService();
        Tokens = new NumberTokenService(Board);
        Harbours = new HarbourService(Board);
        Robber = new Robber();
        Pirate = new Pirate();
        Settlements = new SettlementRegistry();
        Cities = new CityRegistry();
        Roads = new RoadRegistry();
        Ships = new ShipRegistry();
        Resources = new ResourceRegistry();

        var shuffler = new Shuffler(random);
        var mode = createMode(Board, Tokens, Harbours, Robber, Pirate, shuffler);
        mode.Start(players);

        _rules = new PlacementRules(Settlements, Cities, Roads, Ships, Board);
        _setup = new SetupPhase(
            players,
            new PlaceStartingSettlementUseCase(_rules, Settlements),
            new PlaceStartingRoadUseCase(_rules, Roads),
            new GrantStartingResourcesUseCase(Board, Settlements, Resources));
    }

    public bool SetupComplete => _setup.IsComplete;
    public PlayerId? CurrentPlayer => _setup.IsComplete ? null : _setup.Current;

    public MoveResult PlaceStartingSettlementAndRoad(PlayerId player, Vertex settlement, Edge road)
    {
        if (_setup.IsComplete)
            return MoveResult.Rejected("The setup phase is already complete.");
        if (player != _setup.Current)
            return MoveResult.Rejected("It is not your turn.");
        if (!Board.Vertices.Contains(settlement))
            return MoveResult.Rejected("That is not a vertex on this board.");
        if (!_rules.SatisfiesDistanceRule(settlement))
            return MoveResult.Rejected("A settlement must not touch or sit next to another building.");
        if (!Board.Edges.Contains(road))
            return MoveResult.Rejected("That is not an edge on this board.");
        if (!_rules.EdgeIsVacant(road))
            return MoveResult.Rejected("That edge is already occupied.");

        var (a, b) = Board.EndpointsOf(road);
        if (a != settlement && b != settlement)
            return MoveResult.Rejected("Your starting road must touch your new settlement.");

        _setup.PlaceFor(settlement, road);
        return MoveResult.Accepted();
    }
}

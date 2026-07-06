using Catan.Economy;
using Catan.Game;
using Catan.Geometry;
using Catan.Modding;
using Catan.Players;
using Catan.SeafarersScenario1;
using Catan.Standard;

namespace Catan.Tests;

public sealed class GameSessionTests
{
    private static readonly PlayerId[] Players = [new(0), new(1), new(2)];

    private static GameSession NewSession()
    {
        var definition = new BoardDefinitionLoader().Load(Path.Combine(AppContext.BaseDirectory, "modes", "standard.json"));
        return new GameSession(definition, Players, new Random(1));
    }

    [Fact]
    public void A_new_session_starts_at_the_first_player_and_is_not_complete()
    {
        var session = NewSession();

        Assert.False(session.SetupComplete);
        Assert.Equal(Players[0], session.CurrentPlayer);
        Assert.Equal(19, session.Board.Hexes.Count);
    }

    [Fact]
    public void A_move_out_of_turn_is_rejected_and_does_not_advance()
    {
        var session = NewSession();
        var (settlement, road) = FirstLegalMove(session);

        var result = session.PlaceStartingSettlementAndRoad(Players[1], settlement, road);

        Assert.False(result.Success);
        Assert.Equal(Players[0], session.CurrentPlayer);
        Assert.False(session.Settlements.ExistsAt(settlement));
    }

    [Fact]
    public void A_valid_move_is_applied_and_advances_the_turn()
    {
        var session = NewSession();
        var (settlement, road) = FirstLegalMove(session);

        var result = session.PlaceStartingSettlementAndRoad(Players[0], settlement, road);

        Assert.True(result.Success);
        Assert.Equal(Players[0], session.Settlements.At(settlement)!.Owner);
        Assert.Equal(Players[0], session.Roads.At(road)!.Owner);
        Assert.Equal(Players[1], session.CurrentPlayer);
    }

    [Fact]
    public void A_settlement_next_to_an_existing_one_is_rejected_by_the_distance_rule()
    {
        var session = NewSession();
        var (settlement, road) = FirstLegalMove(session);
        session.PlaceStartingSettlementAndRoad(Players[0], settlement, road);

        var neighbour = session.Board.AdjacentVertices(settlement).First(v => session.Board.Vertices.Contains(v));
        var touchingRoad = session.Board.EdgesAround(neighbour).First(e => session.Board.Edges.Contains(e));

        var result = session.PlaceStartingSettlementAndRoad(Players[1], neighbour, touchingRoad);

        Assert.False(result.Success);
        Assert.Contains("next to", result.Error);
    }

    [Fact]
    public void Playing_the_whole_snake_order_completes_setup_and_grants_starting_resources()
    {
        var session = NewSession();

        while (!session.SetupComplete)
        {
            var player = session.CurrentPlayer!.Value;
            var (settlement, road) = FirstLegalMove(session);
            Assert.True(session.PlaceStartingSettlementAndRoad(player, settlement, road).Success);
        }

        Assert.Null(session.CurrentPlayer);
        Assert.Equal(Players.Length * 2, session.Settlements.All.Count);
        Assert.Equal(Players.Length * 2, session.Roads.All.Count);

        int totalResources = Players.Sum(p => Total(session.Resources.Of(p)));
        Assert.True(totalResources > 0, "second-round placements should grant starting resources");
    }

    [Fact]
    public void The_builtin_standard_mode_plays_through_setup()
    {
        var session = new GameSession(
            (board, tokens, harbours, robber, _, shuffler) => new StandardGame(board, tokens, harbours, robber, shuffler),
            Players, new Random(1));

        PlayFullSetup(session);

        Assert.True(session.SetupComplete);
        Assert.Equal(Players.Length * 2, session.Settlements.All.Count);
    }

    [Fact]
    public void The_builtin_seafarers_mode_plays_through_setup()
    {
        var session = new GameSession(
            (board, tokens, harbours, robber, pirate, shuffler) => new SeafarersScenario1Game(board, tokens, harbours, robber, pirate, shuffler),
            Players, new Random(1));

        PlayFullSetup(session);

        Assert.True(session.SetupComplete);
        Assert.Equal(Players.Length * 2, session.Settlements.All.Count);
    }

    [Fact]
    public void A_plugin_mode_builds_its_custom_board_and_accepts_a_placement()
    {
        PlayerId[] two = [new(0), new(1)];
        var session = new GameSession(
            (board, tokens, harbours, robber, pirate, shuffler) => new Catan.Modes.Mini.MiniGame(board, tokens, robber),
            two, new Random(1));

        Assert.Equal(7, session.Board.Hexes.Count);
        Assert.Equal(two[0], session.CurrentPlayer);

        var (settlement, road) = FirstLegalMove(session);
        Assert.True(session.PlaceStartingSettlementAndRoad(two[0], settlement, road).Success);
        Assert.Equal(two[1], session.CurrentPlayer);
    }

    private static void PlayFullSetup(GameSession session)
    {
        while (!session.SetupComplete)
        {
            var player = session.CurrentPlayer!.Value;
            var (settlement, road) = FirstLegalMove(session);
            Assert.True(session.PlaceStartingSettlementAndRoad(player, settlement, road).Success);
        }
    }

    private static (Vertex Settlement, Edge Road) FirstLegalMove(GameSession session)
    {
        foreach (var vertex in session.Board.Vertices)
        {
            if (session.Settlements.ExistsAt(vertex) || session.Cities.ExistsAt(vertex))
                continue;
            if (session.Board.AdjacentVertices(vertex).Any(v => session.Settlements.ExistsAt(v) || session.Cities.ExistsAt(v)))
                continue;

            foreach (var edge in session.Board.EdgesAround(vertex))
            {
                if (!session.Board.Edges.Contains(edge))
                    continue;
                if (session.Roads.ExistsAt(edge) || session.Ships.ExistsAt(edge))
                    continue;

                return (vertex, edge);
            }
        }

        throw new InvalidOperationException("No legal setup move available.");
    }

    private static int Total(ResourceBag bag) => bag.Brick + bag.Lumber + bag.Wool + bag.Grain + bag.Ore;
}

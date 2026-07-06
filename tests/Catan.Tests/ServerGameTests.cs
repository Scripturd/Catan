using Catan.Game;
using Catan.Geometry;
using Catan.Modding;
using Catan.Server;

namespace Catan.Tests;

public sealed class ServerGameTests
{
    private static GameModeRegistration StandardMode()
    {
        var definition = new BoardDefinitionLoader().Load(Path.Combine(AppContext.BaseDirectory, "modes", "standard.json"));
        return new GameModeRegistration(definition.Name, definition.MinPlayers, definition.MaxPlayers,
            (board, tokens, harbours, robber, pirate, shuffler) =>
                new DataDrivenGameMode(definition, board, tokens, harbours, robber, pirate, shuffler));
    }

    private static ServerGame NewGame() => new("ABCDE", StandardMode(), "c0");

    [Fact]
    public void Adding_players_assigns_sequential_ids_and_a_duplicate_connection_is_idempotent()
    {
        var game = NewGame();

        Assert.Equal(0, game.AddPlayer("c0", "Ada").PlayerId);
        Assert.Equal(1, game.AddPlayer("c1", "Ben").PlayerId);
        Assert.Equal(1, game.AddPlayer("c1", "Ben").PlayerId);
    }

    [Fact]
    public void The_lobby_rejects_players_beyond_the_max()
    {
        var game = NewGame();
        game.AddPlayer("c0", "a");
        game.AddPlayer("c1", "b");
        game.AddPlayer("c2", "c");
        game.AddPlayer("c3", "d");

        var fifth = game.AddPlayer("c4", "e");

        Assert.False(fifth.Success);
        Assert.Equal("The game is full.", fifth.Error);
    }

    [Fact]
    public void Start_is_rejected_below_the_minimum_player_count()
    {
        var game = NewGame();
        game.AddPlayer("c0", "a");
        game.AddPlayer("c1", "b");

        var result = game.Start();

        Assert.False(result.Success);
        Assert.Null(game.Session);
        Assert.Equal("Lobby", game.Snapshot().Phase);
    }

    [Fact]
    public void Starting_a_full_enough_lobby_moves_to_the_setup_phase_with_a_board()
    {
        var game = ThreePlayerGame();

        Assert.True(game.Start().Success);

        var snapshot = game.Snapshot();
        Assert.Equal("Setup", snapshot.Phase);
        Assert.Equal(0, snapshot.CurrentPlayerId);
        Assert.NotNull(snapshot.Board);
        Assert.Equal(19, snapshot.Board!.Hexes.Count);
        Assert.NotEmpty(snapshot.Board.Vertices);
        Assert.NotEmpty(snapshot.Board.Edges);
        Assert.All(snapshot.Players, p => Assert.NotNull(p.Hand));
    }

    [Fact]
    public void A_placement_from_the_wrong_connection_is_rejected()
    {
        var game = ThreePlayerGame();
        game.Start();
        var (settlement, road) = FirstLegalMove(game.Session!);

        var result = game.PlaceStarting("c1", settlement, road);

        Assert.False(result.Success);
        Assert.Equal(0, game.Snapshot().CurrentPlayerId);
    }

    [Fact]
    public void A_valid_placement_appears_in_the_snapshot_and_advances_the_turn()
    {
        var game = ThreePlayerGame();
        game.Start();
        var (settlement, road) = FirstLegalMove(game.Session!);

        Assert.True(game.PlaceStarting("c0", settlement, road).Success);

        var snapshot = game.Snapshot();
        Assert.Equal(1, snapshot.CurrentPlayerId);
        var placed = Assert.Single(snapshot.Board!.Settlements);
        Assert.Equal(0, placed.Owner);
        Assert.Equal(PlayerColors.For(0), placed.Color);
    }

    private static ServerGame ThreePlayerGame()
    {
        var game = NewGame();
        game.AddPlayer("c0", "Ada");
        game.AddPlayer("c1", "Ben");
        game.AddPlayer("c2", "Cid");
        return game;
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
                if (session.Board.Edges.Contains(edge) && !session.Roads.ExistsAt(edge) && !session.Ships.ExistsAt(edge))
                    return (vertex, edge);
            }
        }

        throw new InvalidOperationException("No legal setup move available.");
    }
}

using Catan.Board;
using Catan.Economy;
using Catan.GameModes;
using Catan.Geometry;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Tests;

public sealed class DataDrivenGameModeTests
{
    private static readonly PlayerId[] ThreePlayers = [new(0), new(1), new(2)];

    private const string ExplicitBoard = """
    {
      "name": "Tiny",
      "minPlayers": 3,
      "maxPlayers": 4,
      "hexes": [
        { "q": 0, "r": 0, "terrain": "Forest", "token": 6 },
        { "q": 2, "r": 0, "terrain": "Fields", "token": 8 },
        { "q": -2, "r": 0, "terrain": "Desert" }
      ],
      "harbours": [
        { "edge": { "q": 0, "r": 0, "direction": "East" }, "ratio": 3 }
      ]
    }
    """;

    [Fact]
    public void Start_places_terrain_tokens_harbours_and_robber_from_the_definition()
    {
        var (mode, board, tokens, harbours, robber, _) = BuildMode(ExplicitBoard);

        mode.Start(ThreePlayers);

        Assert.Equal(TerrainType.Forest, board.TerrainAt(new Hex(0, 0)));
        Assert.Equal(TerrainType.Fields, board.TerrainAt(new Hex(2, 0)));
        Assert.Equal(TerrainType.Desert, board.TerrainAt(new Hex(-2, 0)));

        Assert.Equal(6, tokens.At(new Hex(0, 0))!.Value.Number);
        Assert.Equal(8, tokens.At(new Hex(2, 0))!.Value.Number);
        Assert.Null(tokens.At(new Hex(-2, 0)));

        var harbour = harbours.At(new Edge(0, 0, EdgeDirection.East));
        Assert.NotNull(harbour);
        Assert.Equal(3, harbour!.Value.Ratio);
        Assert.Null(harbour.Value.Resource);

        Assert.True(robber.IsPlaced);
        Assert.Equal(new Hex(-2, 0), robber.Hex);
    }

    [Fact]
    public void Start_is_idempotent_when_run_twice_on_the_same_services()
    {
        var (mode, board, _, harbours, _, _) = BuildMode(ExplicitBoard);

        mode.Start(ThreePlayers);
        mode.Start(ThreePlayers);

        Assert.Equal(3, board.Hexes.Count);
        Assert.Single(harbours.All);
    }

    [Fact]
    public void Standard_board_file_yields_the_classic_terrain_multiset_tokens_and_harbours()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "modes", "standard.json");
        var definition = new BoardDefinitionLoader().Load(path);
        var (mode, board, tokens, harbours, robber, _) = BuildMode(definition);

        mode.Start(ThreePlayers);

        Assert.Equal(19, board.Hexes.Count);
        Assert.Equal(4, board.HexesOf(TerrainType.Forest).Count());
        Assert.Equal(4, board.HexesOf(TerrainType.Fields).Count());
        Assert.Equal(4, board.HexesOf(TerrainType.Pasture).Count());
        Assert.Equal(3, board.HexesOf(TerrainType.Hills).Count());
        Assert.Equal(3, board.HexesOf(TerrainType.Mountains).Count());
        Assert.Single(board.HexesOf(TerrainType.Desert));

        Assert.Equal(18, board.Hexes.Count(hex => tokens.At(hex) is not null));
        Assert.Equal(9, harbours.All.Count);

        Assert.True(robber.IsPlaced);
        Assert.Equal(TerrainType.Desert, board.TerrainAt(robber.Hex));
    }

    private static (DataDrivenGameMode Mode, BoardService Board, NumberTokenService Tokens, HarbourService Harbours, Robber Robber, Pirate Pirate) BuildMode(string json)
        => BuildMode(new BoardDefinitionLoader().Parse(json, "test"));

    private static (DataDrivenGameMode Mode, BoardService Board, NumberTokenService Tokens, HarbourService Harbours, Robber Robber, Pirate Pirate) BuildMode(BoardDefinition definition)
    {
        var board = new BoardService();
        var tokens = new NumberTokenService(board);
        var harbours = new HarbourService(board);
        var robber = new Robber();
        var pirate = new Pirate();
        var shuffler = new Shuffler(new Random(1));
        var mode = new DataDrivenGameMode(definition, board, tokens, harbours, robber, pirate, shuffler);
        return (mode, board, tokens, harbours, robber, pirate);
    }
}

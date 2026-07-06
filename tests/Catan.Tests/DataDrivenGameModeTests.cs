using Catan.Board;
using Catan.Game;
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
        var (mode, services) = BuildMode(ExplicitBoard);

        mode.Start(services, ThreePlayers);

        Assert.Equal(TerrainType.Forest, services.Board.TerrainAt(new Hex(0, 0)));
        Assert.Equal(TerrainType.Fields, services.Board.TerrainAt(new Hex(2, 0)));
        Assert.Equal(TerrainType.Desert, services.Board.TerrainAt(new Hex(-2, 0)));

        Assert.Equal(6, services.Tokens.At(new Hex(0, 0))!.Value.Number);
        Assert.Equal(8, services.Tokens.At(new Hex(2, 0))!.Value.Number);
        Assert.Null(services.Tokens.At(new Hex(-2, 0)));

        var harbour = services.Harbours.At(new Edge(0, 0, EdgeDirection.East));
        Assert.NotNull(harbour);
        Assert.Equal(3, harbour!.Value.Ratio);
        Assert.Null(harbour.Value.Resource);

        Assert.True(services.Robber.IsPlaced);
        Assert.Equal(new Hex(-2, 0), services.Robber.Hex);
    }

    [Fact]
    public void Standard_board_file_yields_the_classic_terrain_multiset_tokens_and_harbours()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "modes", "standard.json");
        var (mode, services) = BuildMode(new BoardDefinitionLoader().Load(path));

        mode.Start(services, ThreePlayers);

        Assert.Equal(19, services.Board.Hexes.Count);
        Assert.Equal(4, services.Board.HexesOf(TerrainType.Forest).Count());
        Assert.Equal(4, services.Board.HexesOf(TerrainType.Fields).Count());
        Assert.Equal(4, services.Board.HexesOf(TerrainType.Pasture).Count());
        Assert.Equal(3, services.Board.HexesOf(TerrainType.Hills).Count());
        Assert.Equal(3, services.Board.HexesOf(TerrainType.Mountains).Count());
        Assert.Single(services.Board.HexesOf(TerrainType.Desert));

        Assert.Equal(18, services.Board.Hexes.Count(hex => services.Tokens.At(hex) is not null));
        Assert.Equal(9, services.Harbours.All.Count);

        Assert.True(services.Robber.IsPlaced);
        Assert.Equal(TerrainType.Desert, services.Board.TerrainAt(services.Robber.Hex));
    }

    private static (DataDrivenGameMode Mode, GameServices Services) BuildMode(string json)
        => BuildMode(new BoardDefinitionLoader().Parse(json, "test"));

    private static (DataDrivenGameMode Mode, GameServices Services) BuildMode(BoardDefinition definition)
    {
        var board = new BoardService();
        var services = new GameServices(
            board,
            new NumberTokenService(board),
            new HarbourService(board),
            new Robber(),
            new MarkerRegistry(),
            new Shuffler(new Random(1)));
        return (new DataDrivenGameMode(definition), services);
    }
}

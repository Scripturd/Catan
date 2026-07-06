using Catan.Board;
using Catan.Game;
using Catan.Geometry;
using Catan.Pieces;
using Catan.Players;

namespace Catan.Modes.Mini;

public sealed class MiniGame : IGameMode
{
    private static readonly Hex Centre = new(0, 0);
    private static readonly (Hex Hex, TerrainType Terrain, int Token)[] Ring =
    [
        (new Hex(1, 0), TerrainType.Forest, 8),
        (new Hex(0, 1), TerrainType.Fields, 5),
        (new Hex(-1, 1), TerrainType.Pasture, 10),
        (new Hex(-1, 0), TerrainType.Hills, 4),
        (new Hex(0, -1), TerrainType.Mountains, 9),
        (new Hex(1, -1), TerrainType.Forest, 6),
    ];

    private readonly BoardService _board;
    private readonly NumberTokenService _tokens;
    private readonly Robber _robber;

    public int MinPlayerCount => 2;
    public int MaxPlayerCount => 2;

    public MiniGame(BoardService board, NumberTokenService tokens, Robber robber)
    {
        _board = board;
        _tokens = tokens;
        _robber = robber;
    }

    public void Start(IReadOnlyList<PlayerId> players)
    {
        _board.AddHex(Centre, TerrainType.Desert);

        foreach (var (hex, terrain, token) in Ring)
        {
            _board.AddHex(hex, terrain);
            _tokens.Place(hex, new NumberToken(token));
        }

        _robber.Place(Centre);
    }
}

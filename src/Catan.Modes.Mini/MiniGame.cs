using Catan.Board;
using Catan.Game;
using Catan.Geometry;
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

    public string Name => "Mini Duel";
    public int MinPlayerCount => 2;
    public int MaxPlayerCount => 2;

    public void Start(GameServices services, IReadOnlyList<PlayerId> players)
    {
        services.Board.AddHex(Centre, TerrainType.Desert);

        foreach (var (hex, terrain, token) in Ring)
        {
            services.Board.AddHex(hex, terrain);
            services.Tokens.Place(hex, new NumberToken(token));
        }

        services.Robber.Place(Centre);
    }
}

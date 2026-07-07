using Catan.Board;
using Catan.Game;
using Catan.Geometry;
using Catan.Players;

namespace Catan.Standard.Scenario2;

public sealed class Scenario2Game : IGameMode
{
    private static readonly Hex Centre = new(0, 0);
    private static readonly (Hex Hex, TerrainType Terrain, int Token)[] Ring =
    [
        (new Hex(1, 0), StandardTerrain.Forest, 8),
        (new Hex(0, 1), StandardTerrain.Fields, 5),
        (new Hex(-1, 1), StandardTerrain.Pasture, 10),
        (new Hex(-1, 0), StandardTerrain.Hills, 4),
        (new Hex(0, -1), StandardTerrain.Mountains, 9),
        (new Hex(1, -1), StandardTerrain.Forest, 6),
    ];

    public string Name => "Mini Duel";
    public int MinPlayerCount => 2;
    public int MaxPlayerCount => 2;

    public void Start(GameServices services, IReadOnlyList<PlayerId> players)
    {
        services.Board.AddHex(Centre, StandardTerrain.Desert);

        foreach (var (hex, terrain, token) in Ring)
        {
            services.Board.AddHex(hex, terrain);
            services.Tokens.Place(hex, new NumberToken(token));
        }

        services.Robber.Place(Centre);
    }
}

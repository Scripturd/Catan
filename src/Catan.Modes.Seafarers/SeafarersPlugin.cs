using Catan.Game;

namespace Catan.Scenario1;

public sealed class SeafarersPlugin : IGameModePlugin
{
    public IEnumerable<GameModeRegistration> Modes =>
    [
        new GameModeRegistration("Seafarers: Heading for New Shores", 3, 4,
            (board, tokens, harbours, robber, pirate, shuffler) =>
                new Scenario1Game(board, tokens, harbours, robber, pirate, shuffler))
    ];
}

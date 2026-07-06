using Catan.Game;

namespace Catan.Modes.Mini;

public sealed class MiniPack : IExpansionPack
{
    public IEnumerable<GameModeRegistration> Modes =>
    [
        new GameModeRegistration("Mini Duel", 2, 2,
            (board, tokens, harbours, robber, pirate, shuffler) => new MiniGame(board, tokens, robber))
    ];
}

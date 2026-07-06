using Catan.Game;

namespace Catan.Standard;

public sealed class StandardPack : IExpansionPack
{
    public IEnumerable<GameModeRegistration> Modes =>
    [
        new GameModeRegistration("Standard Catan", 3, 4,
            (board, tokens, harbours, robber, pirate, shuffler) =>
                new StandardGame(board, tokens, harbours, robber, shuffler))
    ];
}

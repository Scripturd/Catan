using Catan.Game;
using Catan.Seafarers.Scenario1;

namespace Catan.Seafarers;

public sealed class SeafarersPack : IExpansionPack
{
    public IEnumerable<GameModeRegistration> Modes =>
    [
        new GameModeRegistration("Seafarers: Heading for New Shores", 3, 4,
            (board, tokens, harbours, robber, pirate, shuffler) =>
                new Scenario1Game(board, tokens, harbours, robber, pirate, shuffler))
    ];
}

using Catan.Game;

namespace Catan.Modes.Mini;

public sealed class MiniPack : IExpansionPack
{
    public IEnumerable<IGameMode> Modes => [new MiniGame()];
}

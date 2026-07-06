using Catan.Game;

namespace Catan.Standard;

public sealed class StandardPack : IExpansionPack
{
    public IEnumerable<IGameMode> Modes => [new StandardGame()];
}

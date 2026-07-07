using Catan.Game;
using Catan.Standard.Scenario1;
using Catan.Standard.Scenario2;

namespace Catan.Standard;

public sealed class StandardPack : IExpansionPack
{
    public IEnumerable<IGameMode> Modes => [new Scenario1Game(), new Scenario2Game()];
}

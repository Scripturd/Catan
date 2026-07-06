using Catan.Game;
using Catan.Seafarers.Scenario1;

namespace Catan.Seafarers;

public sealed class SeafarersPack : IExpansionPack
{
    public IEnumerable<IGameMode> Modes => [new Scenario1Game()];
}
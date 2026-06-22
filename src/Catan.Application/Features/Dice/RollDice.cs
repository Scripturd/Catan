using Catan.Application.Ports;
using Catan.Domain.Dice;

namespace Catan.Application.Features.Dice;

/// <summary>
/// A leaf use case: it depends on the <see cref="IRandom"/> port and nothing else.
/// Leaves call no other use cases — that's what keeps the call graph acyclic.
/// </summary>
public sealed class RollDice(IRandom random) : IRollDice
{
    public DiceRoll Execute(RollDiceCommand command) =>
        new(random.RollDie(), random.RollDie());
}

using Catan.Application.Features.Dice;
using Catan.Domain.Dice;

namespace Catan.Application.Tests.TestDoubles;

/// <summary>
/// A stub of the Dice feature. Because the Turn orchestrator composes Dice through
/// the <see cref="IRollDice"/> contract, we can hand it a canned roll and test the
/// orchestrator in complete isolation from the real dice/RNG. That isolation is the
/// whole payoff of composing through public contracts.
/// </summary>
internal sealed class StubRollDice(DiceRoll roll) : IRollDice
{
    public DiceRoll Execute(RollDiceCommand command) => roll;
}

using Catan.Domain.Dice;

namespace Catan.Application.Features.Dice;

/// <summary>
/// The public contract of the Dice feature. Other features compose this feature by
/// depending on THIS interface — never on the <see cref="RollDice"/> class directly.
/// </summary>
public interface IRollDice
{
    DiceRoll Execute(RollDiceCommand command);
}

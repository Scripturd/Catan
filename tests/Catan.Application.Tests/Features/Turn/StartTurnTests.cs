using Catan.Application.Features.Turn;
using Catan.Application.Tests.TestDoubles;
using Catan.Domain.Dice;
using NUnit.Framework;

namespace Catan.Application.Tests.Features.Turn;

/// <summary>
/// The orchestrator (Turn) is tested against a STUBBED Dice feature — no RNG, no real
/// RollDice. This is only possible because Turn depends on the IRollDice contract, not
/// the concrete class. Composition-through-contracts buys you this isolation.
/// </summary>
public class StartTurnTests
{
    [Test]
    public void A_roll_of_seven_activates_the_robber()
    {
        var startTurn = new StartTurn(new StubRollDice(new DiceRoll(3, 4)));

        var result = startTurn.Execute(new StartTurnCommand());

        Assert.That(result.Roll.Total, Is.EqualTo(7));
        Assert.That(result.RobberActivated, Is.True);
    }

    [Test]
    public void Any_other_roll_lets_players_collect()
    {
        var startTurn = new StartTurn(new StubRollDice(new DiceRoll(2, 3)));

        var result = startTurn.Execute(new StartTurnCommand());

        Assert.That(result.RobberActivated, Is.False);
    }
}

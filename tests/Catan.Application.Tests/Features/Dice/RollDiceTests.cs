using Catan.Application.Features.Dice;
using Catan.Application.Tests.TestDoubles;
using NUnit.Framework;

namespace Catan.Application.Tests.Features.Dice;

public class RollDiceTests
{
    [Test]
    public void Rolls_two_dice_from_the_random_port_in_order()
    {
        var random = new QueuedRandom(3, 4);
        var rollDice = new RollDice(random);

        var roll = rollDice.Execute(new RollDiceCommand());

        Assert.That(roll.First, Is.EqualTo(3));
        Assert.That(roll.Second, Is.EqualTo(4));
        Assert.That(roll.Total, Is.EqualTo(7));
    }
}

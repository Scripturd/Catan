using Catan.Application.Features.Dice;

namespace Catan.Application.Features.Turn;

/// <summary>
/// An orchestrator use case. This is the deliberate cross-feature composition: the
/// Turn feature reaches the Dice feature — but ONLY through its public contract
/// (<see cref="IRollDice"/>), injected via the constructor. It never touches the
/// Dice feature's internals, and the dependency runs one way (Turn -> Dice).
///
/// The robber decision is delegated to the domain (<c>roll.ActivatesRobber</c>); the
/// orchestrator only sequences the work, it doesn't own the rule.
/// </summary>
public sealed class StartTurn(IRollDice rollDice) : IStartTurn
{
    public TurnStart Execute(StartTurnCommand command)
    {
        var roll = rollDice.Execute(new RollDiceCommand());
        return new TurnStart(roll, roll.ActivatesRobber);
    }
}

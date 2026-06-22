using Catan.Application.Features.Dice;
using Catan.Application.Features.Turn;
using Catan.Application.Ports;
using Catan.Cli.Adapters;

// Composition root: build the adapters, inject them into the use cases, run a turn.
// This is the ONLY place that knows about concrete implementations.
IRandom random = new SystemRandom();
IRollDice rollDice = new RollDice(random);
IStartTurn startTurn = new StartTurn(rollDice);

var turn = startTurn.Execute(new StartTurnCommand());

Console.WriteLine("Catan (pure .NET) — start of turn");
Console.WriteLine($"  Rolled {turn.Roll.First} + {turn.Roll.Second} = {turn.Roll.Total}");
Console.WriteLine(turn.RobberActivated
    ? "  Seven! The robber activates."
    : "  Players collect resources for this roll.");

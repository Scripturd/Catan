using Catan.Domain.Dice;

namespace Catan.Application.Features.Turn;

/// <summary>
/// The outcome of starting a turn: what was rolled, and whether that roll hands the
/// turn over to the robber instead of resource production.
/// </summary>
public sealed record TurnStart(DiceRoll Roll, bool RobberActivated);

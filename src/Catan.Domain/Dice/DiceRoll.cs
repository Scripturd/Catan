using System;

namespace Catan.Domain.Dice;

/// <summary>
/// The result of rolling the two Catan dice. A value object: validated on
/// construction and immutable. The "7 activates the robber" rule lives here, in the
/// domain — not in a use case — because it's a fact about the dice, not a workflow.
/// </summary>
public sealed record DiceRoll
{
    public int First { get; }
    public int Second { get; }

    public DiceRoll(int first, int second)
    {
        if (first is < 1 or > 6)
            throw new ArgumentOutOfRangeException(nameof(first), first, "A die shows 1..6.");
        if (second is < 1 or > 6)
            throw new ArgumentOutOfRangeException(nameof(second), second, "A die shows 1..6.");

        First = first;
        Second = second;
    }

    public int Total => First + Second;

    public bool ActivatesRobber => Total == 7;
}

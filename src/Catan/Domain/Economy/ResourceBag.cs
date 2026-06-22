namespace Catan.Domain.Economy;

/// <summary>
/// A tally of resources — a player's hand, a build cost, a trade offer, the bank.
/// Immutable and value-equal.
/// </summary>
public readonly record struct ResourceBag(
    int Brick = 0, int Lumber = 0, int Wool = 0, int Grain = 0, int Ore = 0)
{
    public int this[ResourceType r] => r switch
    {
        ResourceType.Brick  => Brick,
        ResourceType.Lumber => Lumber,
        ResourceType.Wool   => Wool,
        ResourceType.Grain  => Grain,
        ResourceType.Ore    => Ore,
        _ => 0
    };

    /// <summary>True if this bag (e.g. a hand) holds at least everything in <paramref name="other"/> (e.g. a cost).</summary>
    public bool Covers(ResourceBag other) =>
        Brick >= other.Brick && Lumber >= other.Lumber && Wool >= other.Wool &&
        Grain >= other.Grain && Ore >= other.Ore;

    public static ResourceBag operator +(ResourceBag a, ResourceBag b) =>
        new(a.Brick + b.Brick, a.Lumber + b.Lumber, a.Wool + b.Wool, a.Grain + b.Grain, a.Ore + b.Ore);

    public static ResourceBag operator -(ResourceBag a, ResourceBag b) =>
        new(a.Brick - b.Brick, a.Lumber - b.Lumber, a.Wool - b.Wool, a.Grain - b.Grain, a.Ore - b.Ore);
}

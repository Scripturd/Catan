namespace Catan.Domain.Economy;

public readonly record struct ResourceBag(
    int Brick = 0, int Lumber = 0, int Wool = 0, int Grain = 0, int Ore = 0)
{
    public int this[ResourceKind r] => r switch
    {
        ResourceKind.Brick  => Brick,
        ResourceKind.Lumber => Lumber,
        ResourceKind.Wool   => Wool,
        ResourceKind.Grain  => Grain,
        ResourceKind.Ore    => Ore,
        _ => 0
    };

    public bool Covers(ResourceBag other) =>
        Brick >= other.Brick && Lumber >= other.Lumber && Wool >= other.Wool &&
        Grain >= other.Grain && Ore >= other.Ore;

    public static ResourceBag operator +(ResourceBag a, ResourceBag b) =>
        new(a.Brick + b.Brick, a.Lumber + b.Lumber, a.Wool + b.Wool, a.Grain + b.Grain, a.Ore + b.Ore);

    public static ResourceBag operator -(ResourceBag a, ResourceBag b) =>
        new(a.Brick - b.Brick, a.Lumber - b.Lumber, a.Wool - b.Wool, a.Grain - b.Grain, a.Ore - b.Ore);
}
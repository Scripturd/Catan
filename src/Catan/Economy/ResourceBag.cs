namespace Catan.Economy;

public readonly record struct ResourceBag
{
    public int Brick { get; }
    public int Lumber { get; }
    public int Wool { get; }
    public int Grain { get; }
    public int Ore { get; }

    public ResourceBag(int brick = 0, int lumber = 0, int wool = 0, int grain = 0, int ore = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(brick);
        ArgumentOutOfRangeException.ThrowIfNegative(lumber);
        ArgumentOutOfRangeException.ThrowIfNegative(wool);
        ArgumentOutOfRangeException.ThrowIfNegative(grain);
        ArgumentOutOfRangeException.ThrowIfNegative(ore);
        Brick = brick;
        Lumber = lumber;
        Wool = wool;
        Grain = grain;
        Ore = ore;
    }

    public int this[ResourceKind r] => r switch
    {
        ResourceKind.Brick  => Brick,
        ResourceKind.Lumber => Lumber,
        ResourceKind.Wool   => Wool,
        ResourceKind.Grain  => Grain,
        ResourceKind.Ore    => Ore,
        _ => throw new ArgumentOutOfRangeException(nameof(r), r, null)
    };

    public bool Covers(ResourceBag other) =>
        Brick >= other.Brick && Lumber >= other.Lumber && Wool >= other.Wool &&
        Grain >= other.Grain && Ore >= other.Ore;

    public static ResourceBag operator +(ResourceBag a, ResourceBag b) =>
        new(a.Brick + b.Brick, a.Lumber + b.Lumber, a.Wool + b.Wool, a.Grain + b.Grain, a.Ore + b.Ore);

    public static ResourceBag operator -(ResourceBag a, ResourceBag b) =>
        new(a.Brick - b.Brick, a.Lumber - b.Lumber, a.Wool - b.Wool, a.Grain - b.Grain, a.Ore - b.Ore);
}
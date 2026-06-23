namespace Catan.Economy;

public readonly record struct Yield
{
    public YieldKind Kind { get; }
    public ResourceKind Resource { get; }

    public Yield(YieldKind kind, ResourceKind resource = default)
    {
        Kind = kind;
        Resource = resource;
    }

    public static readonly Yield Nothing = new(YieldKind.Nothing);
    public static readonly Yield PlayersChoice = new(YieldKind.PlayersChoice);
    public static Yield Of(ResourceKind resource) => new(YieldKind.Resource, resource);
}
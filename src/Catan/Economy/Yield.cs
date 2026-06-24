namespace Catan.Economy;

public readonly record struct Yield
{
    public YieldType Type { get; }
    public ResourceType Resource { get; }

    public Yield(YieldType kind, ResourceType resource = default)
    {
        Type = kind;
        Resource = resource;
    }

    public static readonly Yield Nothing = new(YieldType.Nothing);
    public static readonly Yield PlayersChoice = new(YieldType.PlayersChoice);
    public static Yield Of(ResourceType resource) => new(YieldType.Resource, resource);
}
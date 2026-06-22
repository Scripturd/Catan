using Catan.Domain.Economy;

namespace Catan.Domain.Board;

public readonly record struct TerrainYield
{
    public YieldKind Kind { get; }
    public ResourceKind Resource { get; }

    public TerrainYield(YieldKind kind, ResourceKind resource = default)
    {
        Kind = kind;
        Resource = resource;
    }

    public static readonly TerrainYield Nothing = new(YieldKind.Nothing);
    public static readonly TerrainYield PlayersChoice = new(YieldKind.PlayersChoice);
    public static TerrainYield Of(ResourceKind resource) => new(YieldKind.Resource, resource);
}
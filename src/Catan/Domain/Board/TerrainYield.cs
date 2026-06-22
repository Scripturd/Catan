using Catan.Domain.Economy;

namespace Catan.Domain.Board;

public readonly record struct TerrainYield(YieldKind Kind, ResourceType Resource = default)
{
    public static readonly TerrainYield Nothing = new(YieldKind.Nothing);
    public static readonly TerrainYield PlayersChoice = new(YieldKind.PlayersChoice);
    public static TerrainYield Of(ResourceType resource) => new(YieldKind.Resource, resource);
}
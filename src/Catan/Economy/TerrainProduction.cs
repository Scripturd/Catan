namespace Catan.Economy;

public static class TerrainProduction
{
    public static Yield Of(TerrainKind terrain) => terrain switch
    {
        TerrainKind.Forest    => Yield.Of(ResourceKind.Lumber),
        TerrainKind.Hills     => Yield.Of(ResourceKind.Brick),
        TerrainKind.Pasture   => Yield.Of(ResourceKind.Wool),
        TerrainKind.Fields    => Yield.Of(ResourceKind.Grain),
        TerrainKind.Mountains => Yield.Of(ResourceKind.Ore),
        TerrainKind.Desert    => Yield.Nothing,
        TerrainKind.Sea       => Yield.Nothing,
        TerrainKind.Gold      => Yield.PlayersChoice,
        _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
    };
}
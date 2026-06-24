namespace Catan.Economy;

public static class TerrainYields
{
    public static Yield For(TerrainType terrain) => terrain switch
    {
        TerrainType.Forest    => Yield.Of(ResourceType.Lumber),
        TerrainType.Hills     => Yield.Of(ResourceType.Brick),
        TerrainType.Pasture   => Yield.Of(ResourceType.Wool),
        TerrainType.Fields    => Yield.Of(ResourceType.Grain),
        TerrainType.Mountains => Yield.Of(ResourceType.Ore),
        TerrainType.Desert    => Yield.Nothing,
        TerrainType.Sea       => Yield.Nothing,
        TerrainType.Gold      => Yield.PlayersChoice,
        _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
    };
}
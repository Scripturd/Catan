namespace Catan.Economy;

public static class TerrainProduction
{
    public static TerrainYield Of(TerrainKind terrain) => terrain switch
    {
        TerrainKind.Forest    => TerrainYield.Of(ResourceKind.Lumber),
        TerrainKind.Hills     => TerrainYield.Of(ResourceKind.Brick),
        TerrainKind.Pasture   => TerrainYield.Of(ResourceKind.Wool),
        TerrainKind.Fields    => TerrainYield.Of(ResourceKind.Grain),
        TerrainKind.Mountains => TerrainYield.Of(ResourceKind.Ore),
        TerrainKind.Desert    => TerrainYield.Nothing,
        TerrainKind.Sea       => TerrainYield.Nothing,
        TerrainKind.Gold      => TerrainYield.PlayersChoice,
        _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
    };
}
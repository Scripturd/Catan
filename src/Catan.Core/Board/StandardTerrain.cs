using Catan.Economy;

namespace Catan.Board;

public static class StandardTerrain
{
    public static readonly TerrainType Forest = new("Forest", Yield.Of(ResourceType.Lumber), true, "#2f6b3a");
    public static readonly TerrainType Hills = new("Hills", Yield.Of(ResourceType.Brick), true, "#c2693a");
    public static readonly TerrainType Pasture = new("Pasture", Yield.Of(ResourceType.Wool), true, "#8fc25a");
    public static readonly TerrainType Fields = new("Fields", Yield.Of(ResourceType.Grain), true, "#e8c455");
    public static readonly TerrainType Mountains = new("Mountains", Yield.Of(ResourceType.Ore), true, "#8a8d92");
    public static readonly TerrainType Desert = new("Desert", Yield.Nothing, true, "#dcc99a");
}
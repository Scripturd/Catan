using Catan.Board;
using Catan.Economy;

namespace Catan.Seafarers;

public class SeafarersTerrain
{
    public static readonly TerrainType Sea = new("Sea", Yield.Nothing, false, "#2a6f97");
    public static readonly TerrainType Gold = new("Gold", Yield.PlayersChoice, true, "#f4d03f");
}
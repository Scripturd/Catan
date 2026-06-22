namespace Catan.Domain.Board;

/// <summary>
/// What a hex tile is made of. Five terrains map 1:1 to a resource; Desert produces
/// nothing and hosts the robber at the start of the game.
/// </summary>
public enum TerrainType
{
    Forest,    // -> Lumber
    Hills,     // -> Brick
    Pasture,   // -> Wool
    Fields,    // -> Grain
    Mountains, // -> Ore
    Desert     // -> nothing
}

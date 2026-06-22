namespace Catan.Domain.Board;

/// <summary>
/// What a single hex tile is made of. Five terrains map 1:1 to a
/// <see cref="ResourceType"/>; Desert is the odd one out — it produces nothing
/// and is where the robber sits at the start of the game.
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

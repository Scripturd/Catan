using System;
using Catan.Domain.Economy;

namespace Catan.Domain.Board;

public static class TerrainExtensions
{
    /// <summary>
    /// The resource a terrain produces, or <c>null</c> for the Desert. Returning a
    /// nullable keeps "desert produces nothing" explicit instead of adding a None to
    /// <see cref="ResourceType"/>.
    /// </summary>
    public static ResourceType? Produces(this TerrainType terrain) => terrain switch
    {
        TerrainType.Forest    => ResourceType.Lumber,
        TerrainType.Hills     => ResourceType.Brick,
        TerrainType.Pasture   => ResourceType.Wool,
        TerrainType.Fields    => ResourceType.Grain,
        TerrainType.Mountains => ResourceType.Ore,
        TerrainType.Desert    => null,
        _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
    };
}

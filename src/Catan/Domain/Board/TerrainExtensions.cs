using System;
using Catan.Domain.Economy;

namespace Catan.Domain.Board;

public static class TerrainExtensions
{
    public static TerrainYield Produces(this TerrainType terrain) => terrain switch
    {
        TerrainType.Forest => TerrainYield.Of(ResourceType.Lumber),
        TerrainType.Hills => TerrainYield.Of(ResourceType.Brick),
        TerrainType.Pasture => TerrainYield.Of(ResourceType.Wool),
        TerrainType.Fields => TerrainYield.Of(ResourceType.Grain),
        TerrainType.Mountains => TerrainYield.Of(ResourceType.Ore),
        TerrainType.Desert => TerrainYield.Nothing,
        TerrainType.Sea => TerrainYield.Nothing,
        TerrainType.Gold => TerrainYield.PlayersChoice,
        _ => throw new ArgumentOutOfRangeException(nameof(terrain), terrain, null)
    };
}
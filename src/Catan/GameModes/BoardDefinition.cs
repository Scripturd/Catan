using Catan.Economy;

namespace Catan.GameModes;

public sealed class BoardDefinition
{
    public required string Name { get; init; }
    public int MinPlayers { get; init; }
    public int MaxPlayers { get; init; }
    public IReadOnlyList<HexDefinition> Hexes { get; init; } = [];
    public IReadOnlyList<HarbourDefinition> Harbours { get; init; } = [];
    public HexCoord? Robber { get; init; }
    public bool RandomizeTerrain { get; init; }
    public bool RandomizeTokens { get; init; }
}

public sealed class HexDefinition
{
    public int Q { get; init; }
    public int R { get; init; }
    public required TerrainType Terrain { get; init; }
    public int? Token { get; init; }
}

public sealed class HarbourDefinition
{
    public required EdgeCoord Edge { get; init; }
    public int Ratio { get; init; }
    public ResourceType? Resource { get; init; }
}

public sealed class HexCoord
{
    public int Q { get; init; }
    public int R { get; init; }
}

public sealed class EdgeCoord
{
    public int Q { get; init; }
    public int R { get; init; }
    public required EdgeDirection Direction { get; init; }
}

using Catan.Economy;

namespace Catan.SeafarersScenario1;

public class SeafarersScenario1FourPlayerSetup : ISeafarersScenario1Setup
{
    public IReadOnlyList<Hex> MainHexes { get; } =
    [
        new(-2, -1), new(-1, -1), new(0, -1),
        new(-3, 0), new(-2, 0), new(-1, 0), new(0, 0),
        new(-4, 1), new(-3, 1), new(-2, 1), new(-1, 1), new(-0, 1),
        new(-4, 2), new(-3, 2), new(-2, 2), new(-1, 2),
        new(-4, 3), new(-3, 3), new(-2, 3),
    ];
    public IReadOnlyList<TerrainType> MainTerrainTypes { get; } =
    [
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains, TerrainType.Mountains,
        TerrainType.Desert
    ];
    public IReadOnlyList<int> MainTokens { get; } =
    [
        2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12
    ];

    public IReadOnlyList<Hex> SmallHexes { get; } =
    [
        new(-1, -3), new(0, -3), new(1, -3), new(2, -3), new(3, -3),
        new(2, -2), new(3, -2),
        new(2, -1), new(3, -1),
        new(2, 0),
        new(2, 1),
        new(1, 2),
        new(0, 3),
    ];
    public IReadOnlyList<TerrainType> SmallTerrainTypes { get; } =
    [
        TerrainType.Forest,
        TerrainType.Pasture,
        TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains,
        TerrainType.Sea, TerrainType.Sea, TerrainType.Sea, TerrainType.Sea,
        TerrainType.Gold, TerrainType.Gold
    ];
    public IReadOnlyList<int> SmallTokens { get; } =
    [
        2, 3, 4, 5, 6, 8, 9, 10, 11
    ];

    public IReadOnlyList<Hex> SeaHexes { get; } =
    [
        new(-2, -2), new(-1, -2), new(0, -2), new(1, -2),
        new(-3, -1), new(1, -1),
        new(-4, 0), new(1, 0), new(3, 0),
        new(1, 1),
        new(0, 2),
        new(-1, 3),
    ];

    public IReadOnlyList<Edge> HarbourEdges { get; } = 
    [
        new(-2, -2, EdgeDirection.SouthEast),
        new(-1, -2, EdgeDirection.SouthWest),
        new(-1, -2, EdgeDirection.SouthEast),
        new(0, -2, EdgeDirection.SouthWest),
        new(0, -2, EdgeDirection.SouthEast),
        new(1, -2, EdgeDirection.SouthWest),
        new(-3, -1, EdgeDirection.East),
        new(-3, -1, EdgeDirection.SouthEast),
        new(0, -1, EdgeDirection.East),
        new(1, -1, EdgeDirection.SouthWest),
        new(-4, 0, EdgeDirection.East),
        new(-4, 0, EdgeDirection.SouthEast),
        new(0, 0, EdgeDirection.East),
        new(-5, 1, EdgeDirection.East),
        new(-4, 1, EdgeDirection.SouthWest),
        new(0, 1, EdgeDirection.East),
        new(0, 1, EdgeDirection.SouthEast),
        new(-5, 2, EdgeDirection.East),
        new(-4, 2, EdgeDirection.SouthWest),
        new(-1, 2, EdgeDirection.East),
        new(-1, 2, EdgeDirection.SouthEast),
        new(-5, 3, EdgeDirection.East),
        new(-2, 3, EdgeDirection.East),
        new(-4, 3, EdgeDirection.SouthWest),
        new(-4, 3, EdgeDirection.SouthEast),
        new(-3, 3, EdgeDirection.SouthWest),
        new(-3, 3, EdgeDirection.SouthEast),
        new(-2, 3, EdgeDirection.SouthWest),
        new(-2, 3, EdgeDirection.SouthEast),
    ];
    public IReadOnlyList<Harbour> Harbours { get; } =
    [
        new(3), new(3), new(3), new(3),
        new(2, ResourceType.Brick),
        new(2, ResourceType.Lumber),
        new(2, ResourceType.Wool),
        new(2, ResourceType.Grain),
        new(2, ResourceType.Ore),
    ];
}
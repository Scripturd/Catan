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
}
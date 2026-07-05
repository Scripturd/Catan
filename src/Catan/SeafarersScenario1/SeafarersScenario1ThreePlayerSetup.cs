namespace Catan.SeafarersScenario1;

public class SeafarersScenario1ThreePlayerSetup : ISeafarersScenario1Setup
{
    public IReadOnlyList<HexCoordinate> MainCoords { get; } =
    [
        new(-1, 2), new(0, 2),
        new(-2, 3), new(-1, 3), new(0, 3),
        new(-3, 4), new(-2, 4), new(-1, 4), new(0, 4),
        new(-3, 5), new(-2, 5), new(-1, 5),
        new(-3, 6), new(-2, 6)
    ];

    public IReadOnlyList<HexCoordinate> SmallCoords { get; } =
    [
        new(0, 0), new(1, 0), new(2, 0), new(3, 0),
        new(2, 1), new(3, 1),
        new(2, 2), new(3, 2),
        new(2, 3),
        new(2, 4),
        new(1, 5),
        new(0, 6)
    ];

    public IReadOnlyList<HexCoordinate> SeaCoords { get; } =
    [
        new(-1, 1), new(0, 1), new(1, 1),
        new(-2, 2), new(1, 2),
        new(-3, 3), new(1, 3), new(3, 3),
        new(1, 4),
        new(0, 5),
        new(-1, 6)
    ];

    public IReadOnlyList<TerrainType> MainTerrain { get; } =
    [
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains
    ];

    public IReadOnlyList<TerrainType> SmallTerrain { get; } =
    [
        TerrainType.Pasture,
        TerrainType.Fields,
        TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains,
        TerrainType.Sea, TerrainType.Sea, TerrainType.Sea, TerrainType.Sea,
        TerrainType.Gold, TerrainType.Gold
    ];

    public IReadOnlyList<int> MainTokens { get; } =
    [
        2, 3, 4, 5, 5, 6, 6, 8, 8, 9, 10, 10, 11, 11
    ];

    public IReadOnlyList<int> SmallTokens { get; } =
    [
        3, 4, 4, 5, 8, 9, 10, 12
    ];
}
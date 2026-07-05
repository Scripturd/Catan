namespace Catan.SeafarersScenario1;

public interface ISeafarersScenario1Setup
{
    public IReadOnlyList<HexCoordinate> MainCoords { get; }

    public IReadOnlyList<HexCoordinate> SmallCoords { get; }

    public IReadOnlyList<HexCoordinate> SeaCoords { get; }

    public IReadOnlyList<TerrainType> MainTerrain { get; }

    public IReadOnlyList<TerrainType> SmallTerrain { get; }

    public IReadOnlyList<int> MainTokens { get; }

    public IReadOnlyList<int> SmallTokens { get; }
}
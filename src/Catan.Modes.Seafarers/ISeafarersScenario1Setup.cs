using Catan.Board;
using Catan.Geometry;

namespace Catan.SeafarersScenario1;

public interface ISeafarersScenario1Setup
{
    public IReadOnlyList<Hex> MainHexes { get; }
    public IReadOnlyList<TerrainType> MainTerrainTypes { get; }
    public IReadOnlyList<int> MainTokens { get; }

    public IReadOnlyList<Hex> SmallHexes { get; }
    public IReadOnlyList<TerrainType> SmallTerrainTypes { get; }
    public IReadOnlyList<int> SmallTokens { get; }

    public IReadOnlyList<Hex> SeaHexes { get; }

    public IReadOnlyList<Edge> HarbourEdges { get; }
    public IReadOnlyList<Harbour> Harbours { get; }

    public Hex PirateHex { get; }
}

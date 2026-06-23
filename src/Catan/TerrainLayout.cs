namespace Catan;

public sealed class TerrainLayout
{
    private readonly IReadOnlyDictionary<HexId, TerrainKind> _terrains;

    public TerrainLayout(IReadOnlyDictionary<HexId, TerrainKind> terrains)
    {
        _terrains = terrains;
    }

    public TerrainKind At(HexId hex) => _terrains[hex];

    public IEnumerable<HexId> HexesOf(TerrainKind terrain) => _terrains.Where(t => t.Value == terrain).Select(t => t.Key);
}
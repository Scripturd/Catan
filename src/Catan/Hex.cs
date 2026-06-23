using Catan.Economy;

namespace Catan;

public sealed class Hex
{
    public HexId Id { get; }
    public TerrainKind Terrain { get; }
    public int? NumberToken { get; }
    public IReadOnlyList<VertexId> Vertices { get; }
    public IReadOnlyList<EdgeId> Edges { get; }

    public Hex(
        HexId id,
        TerrainKind terrain,
        int? numberToken,
        IReadOnlyList<VertexId> vertices,
        IReadOnlyList<EdgeId> edges)
    {
        var producesNothing = terrain.Produces().Kind == YieldKind.Nothing;
        if (producesNothing && numberToken.HasValue)
            throw new ArgumentException($"{terrain} produces nothing and cannot carry a number token.", nameof(numberToken));
        if (!producesNothing && numberToken is null)
            throw new ArgumentException($"{terrain} must carry a number token.", nameof(numberToken));

        Id = id;
        Terrain = terrain;
        NumberToken = numberToken;
        Vertices = vertices;
        Edges = edges;
    }
}
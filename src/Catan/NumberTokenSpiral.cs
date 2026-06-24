namespace Catan;

internal static class NumberTokenSpiral
{
    private static readonly int[] NumberTokenSequence =
    {
        5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
    };

    private static readonly (int Q, int R)[] Directions =
    {
        (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)
    };

    public static NumberTokenLayout Place(IReadOnlyList<Hex> hexes, Random random)
    {
        var hexIdAt = hexes.ToDictionary(h => (h.Q, h.R), h => h.Id);
        var terrainOf = hexes.ToDictionary(h => h.Id, h => h.TerrainType);

        var tokens = new Dictionary<HexId, NumberToken>();
        int tokenIndex = 0;
        foreach (var hexId in SpiralFromCorner(hexIdAt, random.Next(0, Directions.Length)))
        {
            if (terrainOf[hexId] == TerrainType.Desert)
                continue;

            tokens[hexId] = new NumberToken(NumberTokenSequence[tokenIndex++]);
        }

        return new NumberTokenLayout(tokens);
    }

    private static IEnumerable<HexId> SpiralFromCorner(
        IReadOnlyDictionary<(int Q, int R), HexId> hexIdAt,
        int corner)
    {
        for (int radius = 2; radius >= 1; radius--)
            foreach (var coord in Ring(radius, corner))
                yield return hexIdAt[coord];

        yield return hexIdAt[(0, 0)];
    }

    private static IEnumerable<(int Q, int R)> Ring(int radius, int corner)
    {
        int q = Directions[corner].Q * radius;
        int r = Directions[corner].R * radius;
        for (int side = 0; side < Directions.Length; side++)
        {
            var step = Directions[(corner + 2 + side) % Directions.Length];
            for (int i = 0; i < radius; i++)
            {
                yield return (q, r);
                q += step.Q;
                r += step.R;
            }
        }
    }
}
namespace Catan.StandardBoard;

public class NumberTokenSpiral
{
    private readonly int[] NumberTokenSequence =
    {
        5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
    };

    private readonly (int Q, int R)[] Directions =
    {
        (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)
    };

    private readonly Random _random;

    public NumberTokenSpiral(Random random)
    {
        _random = random;
    }

    public NumberTokenService Place(BoardService grid)
    {
        var numbers = new NumberTokenService(grid);
        int tokenIndex = 0;
        foreach (var hex in SpiralFromCorner(_random.Next(0, Directions.Length)))
        {
            if (grid.TerrainAt(hex) == TerrainType.Desert)
                continue;

            numbers.Place(hex, new NumberToken(NumberTokenSequence[tokenIndex++]));
        }

        return numbers;
    }

    private IEnumerable<HexCoordinate> SpiralFromCorner(int corner)
    {
        for (int radius = 2; radius >= 1; radius--)
            foreach (var coord in Ring(radius, corner))
                yield return coord;

        yield return new HexCoordinate(0, 0);
    }

    private IEnumerable<HexCoordinate> Ring(int radius, int corner)
    {
        int q = Directions[corner].Q * radius;
        int r = Directions[corner].R * radius;
        for (int side = 0; side < Directions.Length; side++)
        {
            var step = Directions[(corner + 2 + side) % Directions.Length];
            for (int i = 0; i < radius; i++)
            {
                yield return new HexCoordinate(q, r);
                q += step.Q;
                r += step.R;
            }
        }
    }
}
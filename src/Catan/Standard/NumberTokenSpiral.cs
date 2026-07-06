using Catan.Economy;

namespace Catan.Standard;

public class NumberTokenSpiral
{
    private readonly int[] NumberTokenSequence =
    {
        5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
    };

    private readonly IReadOnlyList<Hex> Corners =
    [
        new(2, 0), new(2, -2), new(0, -2), new(-2, 0), new(-2, 2), new(0, 2)
    ];

    private static readonly Hex[] AnticlockwiseDirections =
    [
        new(1, 0), new(1, -1), new(0, -1), new(-1, 0), new(-1, 1), new(0, 1)
    ];

    private readonly BoardService _boardService;
    private readonly NumberTokenService _numberTokenService;
    private readonly Shuffler _shuffler;

    public NumberTokenSpiral(
        BoardService boardService,
        NumberTokenService numberTokenService,
        Shuffler shuffler)
    {
        _boardService = boardService;
        _numberTokenService = numberTokenService;
        _shuffler = shuffler;
    }

    public void Place()
    {
        Hex randomCorner = _shuffler.Shuffle(Corners).First();

        int tokenIndex = 0;
        foreach (var hex in SpiralAnticlockwiseFromHex(randomCorner))
        {
            var terrainType = _boardService.TerrainAt(hex);

            if (TerrainYields.For(terrainType) != Yield.Nothing)
                _numberTokenService.Place(hex, new NumberToken(NumberTokenSequence[tokenIndex++]));
        }
    }

    private static IEnumerable<Hex> SpiralAnticlockwiseFromHex(Hex outerCorner)
    {
        int outerRadius = HexGeometry.HexDistance(outerCorner);
        int startSide = Array.FindIndex(AnticlockwiseDirections, direction => direction * outerRadius == outerCorner);

        return Enumerable
            .Range(1, outerRadius)
            .Reverse()
            .SelectMany(radius => RingAnticlockwiseFromSide(radius, startSide))
            .Append(new Hex(0, 0));
    }

    private static IEnumerable<Hex> RingAnticlockwiseFromSide(int radius, int startSide) =>
        Enumerable
            .Range(0, AnticlockwiseDirections.Length)
            .SelectMany(sideOffset => Side(radius, (startSide + sideOffset) % AnticlockwiseDirections.Length));

    private static IEnumerable<Hex> Side(int radius, int cornerIndex)
    {
        Hex corner = AnticlockwiseDirections[cornerIndex] * radius;
        Hex stepAlongSide = AnticlockwiseDirections[(cornerIndex + 2) % AnticlockwiseDirections.Length];

        return Enumerable
            .Range(0, radius)
            .Select(stepsFromCorner => corner + stepAlongSide * stepsFromCorner);
    }
}
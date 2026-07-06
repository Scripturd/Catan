using Catan.Board;
using Catan.Economy;
using Catan.Geometry;

namespace Catan.Standard;

public class NumberTokenSpiral
{
    private readonly int[] _numberTokenSequence =
    {
        5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
    };

    private readonly IReadOnlyList<Hex> _corners =
    [
        new(2, 0), new(2, -2), new(0, -2), new(-2, 0), new(-2, 2), new(0, 2)
    ];

    private static readonly Hex[] _anticlockwiseDirections =
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
        Hex randomCorner = _shuffler.Shuffle(_corners).First();

        int tokenIndex = 0;
        foreach (var hex in SpiralAnticlockwiseFromHex(randomCorner))
        {
            var terrainType = _boardService.TerrainAt(hex);

            if (terrainType.Yield != Yield.Nothing)
                _numberTokenService.Place(hex, new NumberToken(_numberTokenSequence[tokenIndex++]));
        }
    }

    private static IEnumerable<Hex> SpiralAnticlockwiseFromHex(Hex outerCorner)
    {
        int outerRadius = Distance(outerCorner);
        int startSide = Array.FindIndex(_anticlockwiseDirections, direction => direction * outerRadius == outerCorner);

        return Enumerable
            .Range(1, outerRadius)
            .Reverse()
            .SelectMany(radius => RingAnticlockwiseFromSide(radius, startSide))
            .Append(new Hex(0, 0));
    }

    private static IEnumerable<Hex> RingAnticlockwiseFromSide(int radius, int startSide) =>
        Enumerable
            .Range(0, _anticlockwiseDirections.Length)
            .SelectMany(sideOffset => Side(radius, (startSide + sideOffset) % _anticlockwiseDirections.Length));

    private static IEnumerable<Hex> Side(int radius, int cornerIndex)
    {
        Hex corner = _anticlockwiseDirections[cornerIndex] * radius;
        Hex stepAlongSide = _anticlockwiseDirections[(cornerIndex + 2) % _anticlockwiseDirections.Length];

        return Enumerable
            .Range(0, radius)
            .Select(stepsFromCorner => corner + stepAlongSide * stepsFromCorner);
    }

    private static int Distance(Hex hex) =>
        Math.Max(Math.Max(Math.Abs(hex.Q), Math.Abs(hex.R)), Math.Abs(hex.S));
}

using Catan.Economy;

namespace Catan.Board;

public sealed class NumberTokenService
{
    private readonly BoardService _board;
    private readonly Dictionary<Hex, NumberToken> _tokens = [];

    public NumberTokenService(BoardService board)
    {
        _board = board;
    }

    public void Place(Hex hex, NumberToken token)
    {
        var terrain = _board.TerrainAt(hex);
        if (terrain.Yield == Yield.Nothing)
            throw new InvalidOperationException($"Hex of type {terrain} is unproductive and cannot have a number token.");

        _tokens.Add(hex, token);
    }

    public void Clear() => _tokens.Clear();

    public NumberToken? At(Hex hex) => _tokens.TryGetValue(hex, out var token) ? token : null;

    public IEnumerable<Hex> HexesWith(int roll) => _tokens.Where(t => t.Value.Number == roll).Select(t => t.Key);
}
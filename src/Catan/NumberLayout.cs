namespace Catan;

public sealed class NumberLayout
{
    private readonly IReadOnlyDictionary<HexId, NumberToken> _tokens;

    public NumberLayout(IReadOnlyDictionary<HexId, NumberToken> tokens)
    {
        _tokens = tokens;
    }

    public int Count => _tokens.Count;

    public NumberToken? At(HexId hex) => _tokens.TryGetValue(hex, out var token) ? token : null;

    public IEnumerable<HexId> HexesWith(int roll) => _tokens.Where(t => t.Value.Value == roll).Select(t => t.Key);
}
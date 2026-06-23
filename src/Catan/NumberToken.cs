namespace Catan;

public readonly record struct NumberToken
{
    public int Value { get; }

    public NumberToken(int value)
    {
        if (value is < 2 or > 12 or 7)
            throw new ArgumentOutOfRangeException(nameof(value), value, null);

        Value = value;
    }

    public int Pips => 6 - Math.Abs(7 - Value);
}
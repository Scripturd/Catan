namespace Catan.Board;

public readonly record struct NumberToken
{
    public int Number { get; }

    public int Pips => 6 - Math.Abs(7 - Number);

    public NumberToken(int number)
    {
        if (number is < 2 or > 12 or 7)
            throw new ArgumentOutOfRangeException(nameof(number), number, null);

        Number = number;
    }
}
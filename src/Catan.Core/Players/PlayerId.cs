namespace Catan.Players;

public readonly record struct PlayerId
{
    public int Value { get; }

    public PlayerId(int value)
    {
        Value = value;
    }
}
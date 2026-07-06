namespace Catan.Geometry;

public readonly record struct Hex
{
    public int Q { get; }
    public int R { get; }
    public int S => -Q - R;

    public Hex(int q, int r)
    {
        Q = q;
        R = r;
    }

    public static Hex operator +(Hex left, Hex right)
        => new(left.Q + right.Q, left.R + right.R);

    public static Hex operator -(Hex left, Hex right)
        => new(left.Q - right.Q, left.R - right.R);

    public static Hex operator *(Hex hex, int factor)
        => new(hex.Q * factor, hex.R * factor);
}
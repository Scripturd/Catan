namespace Catan.Board;

public readonly record struct HexCoordinate
{
    public int Q { get; }
    public int R { get; }

    public HexCoordinate(int q, int r)
    {
        Q = q;
        R = r;
    }
}
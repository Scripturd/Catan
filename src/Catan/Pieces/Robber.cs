namespace Catan.Pieces;

public class Robber
{
    public HexCoordinate Hex { get; private set; }

    public Robber(HexCoordinate hex)
    {
        Hex = hex;
    }

    public void MoveTo(HexCoordinate hex)
    {
        Hex = hex;
    }
}
namespace Catan.Pieces;

public class Pirate
{
    public HexCoordinate Hex { get; private set; }

    public Pirate(HexCoordinate hex)
    {
        Hex = hex;
    }

    public void MoveTo(HexCoordinate hex)
    {
        Hex = hex;
    }
}
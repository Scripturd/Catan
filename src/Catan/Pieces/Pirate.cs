namespace Catan.Pieces;

public class Pirate
{
    public HexId Hex { get; private set; }

    public Pirate(HexId hex)
    {
        Hex = hex;
    }

    public void MoveTo(HexId hex)
    {
        Hex = hex;
    }
}
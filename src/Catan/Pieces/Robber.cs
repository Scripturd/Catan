namespace Catan.Pieces;

public class Robber
{
    public HexId Hex { get; private set; }

    public Robber(HexId hex)
    {
        Hex = hex;
    }

    public void MoveTo(HexId hex)
    {
        Hex = hex;
    }
}
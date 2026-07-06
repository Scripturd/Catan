namespace Catan.Pieces;

public class Pirate
{
    public Hex Hex { get; private set; }

    public Pirate(Hex hex)
    {
        Hex = hex;
    }

    public void MoveTo(Hex hex)
    {
        Hex = hex;
    }
}
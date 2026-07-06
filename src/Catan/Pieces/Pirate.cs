namespace Catan.Pieces;

public class Pirate
{
    private Hex _hex;

    public Hex Hex => _hex;

    public void MoveTo(Hex hex)
    {
        _hex = hex;
    }
}
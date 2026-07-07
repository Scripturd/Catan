namespace Catan.Pieces;

public class Robber
{
    private Hex? _hex;

    public bool IsPlaced => _hex.HasValue;
    public Hex Hex => _hex ?? throw new InvalidOperationException("The robber is not on the board.");

    public void Place(Hex hex) => _hex = hex;
    public void Remove() => _hex = null;
}
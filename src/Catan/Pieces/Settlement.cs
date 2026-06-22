using Catan.Players;

namespace Catan.Pieces;

public class Settlement
{
    public PlayerId Owner { get; }

    public Settlement(PlayerId owner)
    {
        Owner = owner;
    }
}
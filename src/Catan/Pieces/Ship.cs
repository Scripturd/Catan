using Catan.Players;

namespace Catan.Pieces;

public class Ship
{
    public PlayerId Owner { get; }

    public Ship(PlayerId owner)
    {
        Owner = owner;
    }
}
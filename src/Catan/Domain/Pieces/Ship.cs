using Catan.Domain.Players;

namespace Catan.Domain.Pieces;

public class Ship
{
    public PlayerId Owner { get; }

    public Ship(PlayerId owner)
    {
        Owner = owner;
    }
}
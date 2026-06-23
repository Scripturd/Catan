using Catan.Economy;
using Catan.Players;

namespace Catan.Pieces;

public class Ship
{
    public static readonly ResourceBag Cost = new(lumber: 1, wool: 1);

    public PlayerId Owner { get; }

    public Ship(PlayerId owner)
    {
        Owner = owner;
    }
}
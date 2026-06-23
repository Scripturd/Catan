using Catan.Economy;
using Catan.Players;

namespace Catan.Pieces;

public class City
{
    public static readonly ResourceBag Cost = new(grain: 2, ore: 3);

    public PlayerId Owner { get; }

    public City(PlayerId owner)
    {
        Owner = owner;
    }
}
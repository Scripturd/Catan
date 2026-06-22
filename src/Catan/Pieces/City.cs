using Catan.Players;

namespace Catan.Pieces;

public class City
{
    public PlayerId Owner { get; }

    public City(PlayerId owner)
    {
        Owner = owner;
    }
}
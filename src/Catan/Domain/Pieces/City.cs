using Catan.Domain.Players;

namespace Catan.Domain.Pieces;

public class City
{
    public PlayerId Owner { get; }

    public City(PlayerId owner)
    {
        Owner = owner;
    }
}
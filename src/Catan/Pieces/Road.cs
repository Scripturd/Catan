using Catan.Economy;
using Catan.Players;

namespace Catan.Pieces;

public class Road
{
    public static readonly ResourceBag Cost = new(brick: 1, lumber: 1);

    public PlayerId Owner { get; }

    public Road(PlayerId owner)
    {
        Owner = owner;
    }
}
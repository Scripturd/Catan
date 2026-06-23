using Catan.Economy;
using Catan.Players;

namespace Catan.Pieces;

public class Settlement
{
    public static readonly ResourceBag Cost = new(brick: 1, lumber: 1, wool: 1, grain: 1);

    public PlayerId Owner { get; }

    public Settlement(PlayerId owner)
    {
        Owner = owner;
    }
}
using Catan.Economy;
using Catan.Players;

namespace Catan.Pieces;

public class Settlement
{
    public static readonly ResourceBag Cost = new(
        (ResourceType.Brick, 1), (ResourceType.Lumber, 1), (ResourceType.Wool, 1), (ResourceType.Grain, 1));

    public PlayerId Owner { get; }

    public Settlement(PlayerId owner)
    {
        Owner = owner;
    }
}
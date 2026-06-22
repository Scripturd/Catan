using Catan.Economy;
using Catan.Players;

namespace Catan.Game;

public sealed class ResourceRegistry
{
    private readonly Dictionary<PlayerId, ResourceBag> _hands = new();

    public ResourceBag Of(PlayerId player) => _hands.GetValueOrDefault(player);

    public bool CanAfford(PlayerId player, ResourceBag cost) => Of(player).Covers(cost);

    public void Give(PlayerId player, ResourceBag amount)
    {
        _hands[player] = Of(player) + amount;
    }

    public void Take(PlayerId player, ResourceBag amount)
    {
        if (!CanAfford(player, amount))
            throw new InvalidOperationException($"Player {player.Value} cannot afford {amount}.");

        _hands[player] = Of(player) - amount;
    }
}
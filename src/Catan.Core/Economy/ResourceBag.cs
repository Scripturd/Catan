namespace Catan.Economy;

public sealed class ResourceBag
{
    public static readonly ResourceBag Empty = new();

    private readonly IReadOnlyDictionary<ResourceType, int> _amounts;

    public ResourceBag(params (ResourceType Resource, int Amount)[] amounts)
    {
        var map = new Dictionary<ResourceType, int>();
        foreach (var (resource, amount) in amounts)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount);
            if (amount > 0)
                map[resource] = map.GetValueOrDefault(resource) + amount;
        }
        _amounts = map;
    }

    private ResourceBag(IReadOnlyDictionary<ResourceType, int> amounts) => _amounts = amounts;

    public static ResourceBag Of(ResourceType resource, int amount) => new((resource, amount));

    public int this[ResourceType resource] => _amounts.GetValueOrDefault(resource);

    public int Total => _amounts.Values.Sum();

    public IReadOnlyDictionary<ResourceType, int> Amounts => _amounts;

    public bool Covers(ResourceBag other) => other._amounts.All(entry => this[entry.Key] >= entry.Value);

    public static ResourceBag operator +(ResourceBag a, ResourceBag b) => Combine(a, b, +1);
    public static ResourceBag operator -(ResourceBag a, ResourceBag b) => Combine(a, b, -1);

    private static ResourceBag Combine(ResourceBag a, ResourceBag b, int sign)
    {
        var map = new Dictionary<ResourceType, int>(a._amounts);
        foreach (var (resource, amount) in b._amounts)
        {
            var total = map.GetValueOrDefault(resource) + sign * amount;
            if (total < 0)
                throw new InvalidOperationException($"A bag cannot hold a negative amount of {resource.Name}.");
            if (total == 0)
                map.Remove(resource);
            else
                map[resource] = total;
        }
        return new ResourceBag(map);
    }

    public override string ToString() =>
        _amounts.Count == 0 ? "nothing" : string.Join(", ", _amounts.Select(entry => $"{entry.Value} {entry.Key.Name}"));
}

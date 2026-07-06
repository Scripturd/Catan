namespace Catan.Economy;

public sealed class ResourceType
{
    public string Name { get; }
    public string Color { get; }

    public ResourceType(string name, string color)
    {
        Name = name;
        Color = color;
    }

    public static readonly ResourceType Brick = new("Brick", "#c2693a");
    public static readonly ResourceType Lumber = new("Lumber", "#2f6b3a");
    public static readonly ResourceType Wool = new("Wool", "#8fc25a");
    public static readonly ResourceType Grain = new("Grain", "#e8c455");
    public static readonly ResourceType Ore = new("Ore", "#8a8d92");

    private static readonly IReadOnlyDictionary<string, ResourceType> BuiltIns =
        new[] { Brick, Lumber, Wool, Grain, Ore }.ToDictionary(r => r.Name, StringComparer.OrdinalIgnoreCase);

    public static ResourceType? ByName(string name) => BuiltIns.GetValueOrDefault(name);

    public override bool Equals(object? obj) => obj is ResourceType other && Name == other.Name;
    public override int GetHashCode() => Name.GetHashCode();
    public static bool operator ==(ResourceType? a, ResourceType? b) => Equals(a, b);
    public static bool operator !=(ResourceType? a, ResourceType? b) => !Equals(a, b);
    public override string ToString() => Name;
}

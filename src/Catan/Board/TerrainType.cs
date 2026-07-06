using Catan.Economy;

namespace Catan.Board;

public sealed class TerrainType
{
    public string Name { get; }
    public Yield Yield { get; }
    public bool IsLand { get; }
    public string Color { get; }

    public TerrainType(string name, Yield yield, bool isLand, string color)
    {
        Name = name;
        Yield = yield;
        IsLand = isLand;
        Color = color;
    }

    public static readonly TerrainType Forest = new("Forest", Yield.Of(ResourceType.Lumber), true, "#2f6b3a");
    public static readonly TerrainType Hills = new("Hills", Yield.Of(ResourceType.Brick), true, "#c2693a");
    public static readonly TerrainType Pasture = new("Pasture", Yield.Of(ResourceType.Wool), true, "#8fc25a");
    public static readonly TerrainType Fields = new("Fields", Yield.Of(ResourceType.Grain), true, "#e8c455");
    public static readonly TerrainType Mountains = new("Mountains", Yield.Of(ResourceType.Ore), true, "#8a8d92");
    public static readonly TerrainType Desert = new("Desert", Yield.Nothing, true, "#dcc99a");
    public static readonly TerrainType Sea = new("Sea", Yield.Nothing, false, "#2a6f97");
    public static readonly TerrainType Gold = new("Gold", Yield.PlayersChoice, true, "#f4d03f");

    private static readonly IReadOnlyDictionary<string, TerrainType> BuiltIns =
        new[] { Forest, Hills, Pasture, Fields, Mountains, Desert, Sea, Gold }
            .ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

    public static TerrainType? ByName(string name) => BuiltIns.GetValueOrDefault(name);

    public bool Equals(TerrainType? other) => other is not null && Name == other.Name;
    public override bool Equals(object? obj) => Equals(obj as TerrainType);
    public override int GetHashCode() => Name.GetHashCode();
    public static bool operator ==(TerrainType? a, TerrainType? b) => Equals(a, b);
    public static bool operator !=(TerrainType? a, TerrainType? b) => !Equals(a, b);
    public override string ToString() => Name;
}

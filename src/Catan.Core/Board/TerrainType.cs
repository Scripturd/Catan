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

    public bool Equals(TerrainType? other) => other is not null && Name == other.Name;
    public override bool Equals(object? obj) => Equals(obj as TerrainType);
    public override int GetHashCode() => Name.GetHashCode();
    public static bool operator ==(TerrainType? a, TerrainType? b) => Equals(a, b);
    public static bool operator !=(TerrainType? a, TerrainType? b) => !Equals(a, b);
    public override string ToString() => Name;
}
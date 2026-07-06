namespace Catan.Pieces;

public sealed class Marker
{
    public string Kind { get; }
    public Hex Hex { get; }
    public string Color { get; }
    public string Glyph { get; }

    public Marker(string kind, Hex hex, string color, string glyph)
    {
        Kind = kind;
        Hex = hex;
        Color = color;
        Glyph = glyph;
    }
}

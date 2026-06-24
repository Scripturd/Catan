namespace Catan.Cli;

internal static class BoardRenderer
{
    public static string ToText(HexGrid grid, NumberTokenLayout numbers)
    {
        int minRow = grid.Hexes.Min(h => h.R);
        int minSlot = grid.Hexes.Min(h => h.Q * 2 + h.R);
        int maxSlot = grid.Hexes.Max(h => h.Q * 2 + h.R);
        int width = (maxSlot - minSlot) * 3 + 6;

        var lines = new Dictionary<int, char[]>();
        foreach (var hex in grid.Hexes)
        {
            int line = hex.R - minRow;
            int col = (hex.Q * 2 + hex.R - minSlot) * 3;
            if (!lines.TryGetValue(line, out var chars))
            {
                chars = new char[width];
                Array.Fill(chars, ' ');
                lines[line] = chars;
            }

            var label = Label(hex, numbers);
            for (int i = 0; i < label.Length && col + i < width; i++)
                chars[col + i] = label[i];
        }

        return string.Join("\n", lines.OrderBy(l => l.Key).Select(l => new string(l.Value).TrimEnd()));
    }

    private static string Label(Hex hex, NumberTokenLayout numbers)
    {
        if (hex.TerrainType == TerrainType.Sea)
            return "~";

        var token = numbers.At(hex.Id);
        return Abbrev(hex.TerrainType) + (token.HasValue ? token.Value.Number.ToString() : "");
    }

    private static string Abbrev(TerrainType terrain) => terrain switch
    {
        TerrainType.Forest => "Fo",
        TerrainType.Pasture => "Pa",
        TerrainType.Fields => "Fi",
        TerrainType.Hills => "Hi",
        TerrainType.Mountains => "Mo",
        TerrainType.Desert => "De",
        TerrainType.Gold => "Go",
        TerrainType.Sea => "~",
        _ => "?"
    };
}
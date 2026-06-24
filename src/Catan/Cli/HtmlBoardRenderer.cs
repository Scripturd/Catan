using System.Globalization;
using System.Text;

namespace Catan.Cli;

internal static class HtmlBoardRenderer
{
    private const double Size = 60;
    private const double Margin = 40;

    public static string ToHtml(HexGrid grid, NumberTokenLayout numbers)
    {
        var centres = grid.Hexes.ToDictionary(
            h => h.Id,
            h => (X: Size * Math.Sqrt(3) * (h.Q + h.R / 2.0), Y: Size * 1.5 * h.R));

        double minX = centres.Values.Min(c => c.X) - Size - Margin;
        double minY = centres.Values.Min(c => c.Y) - Size - Margin;
        double maxX = centres.Values.Max(c => c.X) + Size + Margin;
        double maxY = centres.Values.Max(c => c.Y) + Size + Margin;
        double width = maxX - minX;
        double height = maxY - minY;

        var svg = new StringBuilder();
        svg.Append(F(
            "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"{0} {1} {2} {3}\" width=\"{2}\" height=\"{3}\">",
            minX, minY, width, height));

        foreach (var hex in grid.Hexes)
        {
            var (cx, cy) = centres[hex.Id];
            svg.Append(Hexagon(cx, cy, Fill(hex.TerrainType)));

            if (hex.TerrainType == TerrainType.Sea)
                continue;

            var token = numbers.At(hex.Id);
            if (token.HasValue)
                svg.Append(Token(cx, cy, token.Value));
        }

        svg.Append("</svg>");

        return "<!doctype html>\n<html lang=\"en\">\n<head>\n<meta charset=\"utf-8\">\n" +
            "<title>Catan Board</title>\n<style>\n" +
            "  body { margin: 0; background: #1d4e6f; display: grid; place-items: center;" +
            " min-height: 100vh; font-family: system-ui, sans-serif; }\n" +
            "  svg { max-width: 100%; height: auto; }\n</style>\n</head>\n<body>" +
            svg + "</body>\n</html>";
    }

    private static string Hexagon(double cx, double cy, string fill)
    {
        var points = new StringBuilder();
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 180.0 * (60 * i - 30);
            double px = cx + Size * Math.Cos(angle);
            double py = cy + Size * Math.Sin(angle);
            if (i > 0)
                points.Append(' ');
            points.Append(F("{0},{1}", px, py));
        }

        return F(
            "<polygon points=\"{0}\" fill=\"{1}\" stroke=\"#0d2c40\" stroke-width=\"2\"/>",
            points.ToString(), fill);
    }

    private static string Token(double cx, double cy, NumberToken token)
    {
        string colour = token.Number is 6 or 8 ? "#b8312f" : "#1a1a1a";
        var pips = new string('•', token.Pips);
        return F(
            "<circle cx=\"{0}\" cy=\"{1}\" r=\"22\" fill=\"#f1e3c0\" stroke=\"#0d2c40\" stroke-width=\"1.5\"/>" +
            "<text x=\"{0}\" y=\"{2}\" text-anchor=\"middle\" font-size=\"22\" font-weight=\"700\" fill=\"{3}\">{4}</text>" +
            "<text x=\"{0}\" y=\"{5}\" text-anchor=\"middle\" font-size=\"11\" fill=\"{3}\">{6}</text>",
            cx, cy, cy + 4, colour, token.Number, cy + 18, pips);
    }

    private static string Fill(TerrainType terrain) => terrain switch
    {
        TerrainType.Forest => "#2f6b3a",
        TerrainType.Pasture => "#8fc25a",
        TerrainType.Fields => "#e8c455",
        TerrainType.Hills => "#c2693a",
        TerrainType.Mountains => "#8a8d92",
        TerrainType.Desert => "#dcc99a",
        TerrainType.Gold => "#f4d03f",
        TerrainType.Sea => "#2a6f97",
        _ => "#888888"
    };

    private static string F(string format, params object[] args) =>
        string.Format(CultureInfo.InvariantCulture, format, args);
}

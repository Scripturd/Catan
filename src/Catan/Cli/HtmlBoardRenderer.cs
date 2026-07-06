using Catan.Economy;
using Catan.Pieces;
using System.Globalization;
using System.Text;

namespace Catan.Cli;

internal static class HtmlBoardRenderer
{
    private const double Size = 60;
    private const double Margin = 70;

    public static string ToHtml(BoardService grid, NumberTokenService numbers, HarbourService harbours, Robber robber, Pirate? pirate)
    {
        var centres = grid.Hexes.ToDictionary(h => h, HexCentre);

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
            var (cx, cy) = centres[hex];
            var terrain = grid.TerrainAt(hex);
            svg.Append(Hexagon(cx, cy, Fill(terrain)));

            if (terrain == TerrainType.Sea)
                continue;

            var token = numbers.At(hex);
            if (token.HasValue)
                svg.Append(Token(cx, cy, token.Value));
        }

        foreach (var (edge, harbour) in harbours.All)
            svg.Append(HarbourMarker(grid, edge, harbour));

        if (centres.TryGetValue(robber.Hex, out var robberCentre))
            svg.Append(RobberPawn(robberCentre.X, robberCentre.Y));

        if (pirate is not null && centres.TryGetValue(pirate.Hex, out var pirateCentre))
            svg.Append(PirateShip(pirateCentre.X, pirateCentre.Y));

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

    private static string RobberPawn(double cx, double cy)
    {
        double neckY = cy - 10;
        double shoulderY = cy - 4;
        double waistY = cy + 2;
        double hipY = cy + 8;
        double baseY = cy + 16;

        string body = F(
            "<path d=\"M {0},{5} C {1},{6} {2},{7} {3},{8} L {4},{9} L {10},{9} L {11},{8} C {12},{7} {13},{6} {14},{5} Z\"" +
            " fill=\"#2b2b2b\" stroke=\"#f1e3c0\" stroke-width=\"1.5\"/>",
            cx - 5, cx - 9, cx - 8, cx - 11, cx - 14,
            neckY, shoulderY, waistY, hipY, baseY,
            cx + 14, cx + 11, cx + 8, cx + 9, cx + 5);

        return F(
            "<ellipse cx=\"{0}\" cy=\"{1}\" rx=\"15\" ry=\"4\" fill=\"#2b2b2b\" stroke=\"#f1e3c0\" stroke-width=\"1.5\"/>",
            cx, baseY) +
            body +
            F("<circle cx=\"{0}\" cy=\"{1}\" r=\"8\" fill=\"#2b2b2b\" stroke=\"#f1e3c0\" stroke-width=\"1.5\"/>",
                cx, cy - 20);
    }

    private static string PirateShip(double cx, double cy)
    {
        string hull = F(
            "<path d=\"M {0},{2} Q {6},{3} {1},{2} L {4},{5} Q {6},{7} {8},{5} Z\"" +
            " fill=\"#2b2b2b\" stroke=\"#f1e3c0\" stroke-width=\"1.5\"/>",
            cx - 20, cx + 20, cy + 2, cy + 4,
            cx + 13, cy + 16, cx, cy + 20, cx - 13);

        string mast = F(
            "<line x1=\"{0}\" y1=\"{1}\" x2=\"{0}\" y2=\"{2}\" stroke=\"#f1e3c0\" stroke-width=\"2\"/>",
            cx, cy + 2, cy - 26);

        string sail = F(
            "<path d=\"M {0},{1} L {0},{2} Q {3},{4} {0},{1} Z\"" +
            " fill=\"#2b2b2b\" stroke=\"#f1e3c0\" stroke-width=\"1.5\"/>",
            cx + 1, cy - 24, cy - 3, cx + 18, cy - 13);

        return mast + sail + hull;
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

    private static string HarbourMarker(BoardService grid, Edge edge, Harbour harbour)
    {
        var (a, b) = grid.EndpointsOf(edge);
        var (ax, ay) = VertexPixel(a);
        var (bx, by) = VertexPixel(b);
        double mx = (ax + bx) / 2;
        double my = (ay + by) / 2;

        var landHex = HexGeometry.HexesOf(edge).First(h => grid.Hexes.Contains(h) && grid.TerrainAt(h) != TerrainType.Sea);
        var (lx, ly) = HexCentre(landHex);
        double dx = mx - lx;
        double dy = my - ly;
        double length = Math.Sqrt(dx * dx + dy * dy);

        double hx = mx + dx / length * Size * 0.55;
        double hy = my + dy / length * Size * 0.55;

        return F(
            "<line x1=\"{0}\" y1=\"{1}\" x2=\"{4}\" y2=\"{5}\" stroke=\"#7a5230\" stroke-width=\"3\"/>" +
            "<line x1=\"{2}\" y1=\"{3}\" x2=\"{4}\" y2=\"{5}\" stroke=\"#7a5230\" stroke-width=\"3\"/>" +
            "<circle cx=\"{4}\" cy=\"{5}\" r=\"16\" fill=\"{6}\" stroke=\"#0d2c40\" stroke-width=\"1.5\"/>" +
            "<text x=\"{4}\" y=\"{7}\" text-anchor=\"middle\" font-size=\"12\" font-weight=\"700\" fill=\"#1a1a1a\">{8}:1</text>",
            ax, ay, bx, by, hx, hy, HarbourFill(harbour), hy + 4, harbour.Ratio);
    }

    private static (double X, double Y) VertexPixel(Vertex vertex)
    {
        var (cx, cy) = HexCentre(new Hex(vertex.Q, vertex.R));
        return (cx, cy + (vertex.Corner == VertexCorner.Top ? Size : -Size));
    }

    private static (double X, double Y) HexCentre(Hex hex) =>
        (Size * Math.Sqrt(3) * (hex.Q + hex.R / 2.0), Size * 1.5 * hex.R);

    private static string HarbourFill(Harbour harbour) => harbour.Resource switch
    {
        null => "#e8dcc0",
        ResourceType.Brick => "#c2693a",
        ResourceType.Lumber => "#2f6b3a",
        ResourceType.Wool => "#8fc25a",
        ResourceType.Grain => "#e8c455",
        ResourceType.Ore => "#8a8d92",
        _ => "#888888"
    };

    private static string F(string format, params object[] args) =>
        string.Format(CultureInfo.InvariantCulture, format, args);
}
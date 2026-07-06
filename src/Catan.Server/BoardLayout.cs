using Catan.Geometry;

namespace Catan.Server;

public static class BoardLayout
{
    public const double Size = 60;
    public const double Margin = 70;

    public static (double X, double Y) HexCentre(Hex hex) =>
        (Size * Math.Sqrt(3) * (hex.Q + hex.R / 2.0), Size * 1.5 * hex.R);

    public static (double X, double Y) VertexPixel(Vertex vertex)
    {
        var (cx, cy) = HexCentre(new Hex(vertex.Q, vertex.R));
        return (cx, cy + (vertex.Corner == VertexCorner.Top ? Size : -Size));
    }
}

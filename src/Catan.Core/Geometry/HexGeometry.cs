namespace Catan.Geometry;

internal static class HexGeometry
{
    public static bool IsOrigin(Hex hex)
        => hex.Q == 0 && hex.R == 0;

    public static int HexDistance(Hex hex)
        => Math.Max(Math.Max(Math.Abs(hex.Q), Math.Abs(hex.R)), Math.Abs(hex.S));

    public static Vertex[] VerticesOf(Hex hex)
    {
        int q = hex.Q;
        int r = hex.R;
        return
        [
            new Vertex(q + 1, r - 1, VertexCorner.Top),
            new Vertex(q, r + 1, VertexCorner.Bottom),
            new Vertex(q, r, VertexCorner.Top),
            new Vertex(q - 1, r + 1, VertexCorner.Bottom),
            new Vertex(q, r - 1, VertexCorner.Top),
            new Vertex(q, r, VertexCorner.Bottom)
        ];
    }

    public static Edge[] EdgesOf(Hex hex)
    {
        int q = hex.Q;
        int r = hex.R;
        return
        [
            new Edge(q, r, EdgeDirection.East),
            new Edge(q, r, EdgeDirection.SouthEast),
            new Edge(q, r, EdgeDirection.SouthWest),
            new Edge(q - 1, r, EdgeDirection.East),
            new Edge(q, r - 1, EdgeDirection.SouthEast),
            new Edge(q + 1, r - 1, EdgeDirection.SouthWest)
        ];
    }

    public static (Vertex A, Vertex B) EndpointsOf(Edge edge)
    {
        int q = edge.Q;
        int r = edge.R;
        return edge.Direction switch
        {
            EdgeDirection.East => (new Vertex(q + 1, r - 1, VertexCorner.Top), new Vertex(q, r + 1, VertexCorner.Bottom)),
            EdgeDirection.SouthEast => (new Vertex(q, r + 1, VertexCorner.Bottom), new Vertex(q, r, VertexCorner.Top)),
            EdgeDirection.SouthWest => (new Vertex(q, r, VertexCorner.Top), new Vertex(q - 1, r + 1, VertexCorner.Bottom)),
            _ => throw new ArgumentOutOfRangeException(nameof(edge))
        };
    }

    public static Hex[] HexesOf(Edge edge)
    {
        int q = edge.Q;
        int r = edge.R;
        return edge.Direction switch
        {
            EdgeDirection.East => [new Hex(q, r), new Hex(q + 1, r)],
            EdgeDirection.SouthEast => [new Hex(q, r), new Hex(q, r + 1)],
            EdgeDirection.SouthWest => [new Hex(q, r), new Hex(q - 1, r + 1)],
            _ => throw new ArgumentOutOfRangeException(nameof(edge))
        };
    }

    public static Hex[] HexesAround(Vertex vertex)
    {
        int q = vertex.Q;
        int r = vertex.R;
        return vertex.Corner switch
        {
            VertexCorner.Top =>
            [
                new Hex(q, r),
                new Hex(q - 1, r + 1),
                new Hex(q, r + 1)
            ],
            VertexCorner.Bottom =>
            [
                new Hex(q, r),
                new Hex(q, r - 1),
                new Hex(q + 1, r - 1)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(vertex))
        };
    }

    public static Edge[] EdgesAround(Vertex vertex)
    {
        int q = vertex.Q;
        int r = vertex.R;
        return vertex.Corner switch
        {
            VertexCorner.Top =>
            [
                new Edge(q - 1, r + 1, EdgeDirection.East),
                new Edge(q, r, EdgeDirection.SouthEast),
                new Edge(q, r, EdgeDirection.SouthWest)
            ],
            VertexCorner.Bottom =>
            [
                new Edge(q, r - 1, EdgeDirection.East),
                new Edge(q, r - 1, EdgeDirection.SouthEast),
                new Edge(q + 1, r - 1, EdgeDirection.SouthWest)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(vertex))
        };
    }

    public static Vertex Opposite(Edge edge, Vertex vertex)
    {
        var (a, b) = EndpointsOf(edge);
        if (vertex == a)
            return b;
        if (vertex == b)
            return a;

        throw new ArgumentException($"Vertex {vertex} is not an endpoint of edge {edge}.", nameof(vertex));
    }
}
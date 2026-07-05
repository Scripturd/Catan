namespace Catan.Board;

internal static class HexGeometry
{
    public static VertexCoordinate[] VerticesOf(HexCoordinate hex)
    {
        int q = hex.Q;
        int r = hex.R;
        return
        [
            Vertex(q + 1, r - 1, VertexCorner.Top),
            Vertex(q, r + 1, VertexCorner.Bottom),
            Vertex(q, r, VertexCorner.Top),
            Vertex(q - 1, r + 1, VertexCorner.Bottom),
            Vertex(q, r - 1, VertexCorner.Top),
            Vertex(q, r, VertexCorner.Bottom)
        ];
    }

    public static EdgeCoordinate[] EdgesOf(HexCoordinate hex)
    {
        int q = hex.Q;
        int r = hex.R;
        return
        [
            Edge(q, r, EdgeDirection.East),
            Edge(q, r, EdgeDirection.NorthEast),
            Edge(q, r, EdgeDirection.NorthWest),
            Edge(q - 1, r, EdgeDirection.East),
            Edge(q, r - 1, EdgeDirection.NorthEast),
            Edge(q + 1, r - 1, EdgeDirection.NorthWest)
        ];
    }

    public static (VertexCoordinate A, VertexCoordinate B) EndpointsOf(EdgeCoordinate edge)
    {
        int q = edge.Q;
        int r = edge.R;
        return edge.Direction switch
        {
            EdgeDirection.East => (Vertex(q + 1, r - 1, VertexCorner.Top), Vertex(q, r + 1, VertexCorner.Bottom)),
            EdgeDirection.NorthEast => (Vertex(q, r + 1, VertexCorner.Bottom), Vertex(q, r, VertexCorner.Top)),
            EdgeDirection.NorthWest => (Vertex(q, r, VertexCorner.Top), Vertex(q - 1, r + 1, VertexCorner.Bottom)),
            _ => throw new ArgumentOutOfRangeException(nameof(edge))
        };
    }

    public static HexCoordinate[] HexesAround(VertexCoordinate vertex)
    {
        int q = vertex.Q;
        int r = vertex.R;
        return vertex.Corner switch
        {
            VertexCorner.Top =>
            [
                Hex(q, r),
                Hex(q - 1, r + 1),
                Hex(q, r + 1)
            ],
            VertexCorner.Bottom =>
            [
                Hex(q, r),
                Hex(q, r - 1),
                Hex(q + 1, r - 1)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(vertex))
        };
    }

    public static EdgeCoordinate[] EdgesAround(VertexCoordinate vertex)
    {
        int q = vertex.Q;
        int r = vertex.R;
        return vertex.Corner switch
        {
            VertexCorner.Top =>
            [
                Edge(q - 1, r + 1, EdgeDirection.East),
                Edge(q, r, EdgeDirection.NorthEast),
                Edge(q, r, EdgeDirection.NorthWest)
            ],
            VertexCorner.Bottom =>
            [
                Edge(q, r - 1, EdgeDirection.East),
                Edge(q, r - 1, EdgeDirection.NorthEast),
                Edge(q + 1, r - 1, EdgeDirection.NorthWest)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(vertex))
        };
    }

    private static HexCoordinate Hex(int q, int r) => new(q, r);

    private static VertexCoordinate Vertex(int q, int r, VertexCorner corner) => new(q, r, corner);

    private static EdgeCoordinate Edge(int q, int r, EdgeDirection direction) => new(q, r, direction);
}
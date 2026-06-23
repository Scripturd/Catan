namespace Catan;

public static class StandardBoard
{
    private static readonly TerrainKind[] Terrains =
    {
        TerrainKind.Desert,
        TerrainKind.Forest, TerrainKind.Forest, TerrainKind.Forest, TerrainKind.Forest,
        TerrainKind.Fields, TerrainKind.Fields, TerrainKind.Fields, TerrainKind.Fields,
        TerrainKind.Pasture, TerrainKind.Pasture, TerrainKind.Pasture, TerrainKind.Pasture,
        TerrainKind.Hills, TerrainKind.Hills, TerrainKind.Hills,
        TerrainKind.Mountains, TerrainKind.Mountains, TerrainKind.Mountains
    };

    private static readonly int[] NumberTokens =
    {
        2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12
    };

    public static Board Create()
    {
        var hexes = new List<(int Q, int R)>();
        for (int q = -2; q <= 2; q++)
            for (int r = -2; r <= 2; r++)
                if (Math.Abs(q + r) <= 2)
                    hexes.Add((q, r));

        var vertexIds = new Dictionary<(long, long), VertexId>();
        var edgeIds = new Dictionary<(int, int), EdgeId>();
        var vertexHexes = new Dictionary<VertexId, HashSet<HexId>>();
        var vertexEdges = new Dictionary<VertexId, HashSet<EdgeId>>();
        var vertexAdjacent = new Dictionary<VertexId, HashSet<VertexId>>();
        var edgeEnds = new Dictionary<EdgeId, (VertexId A, VertexId B)>();
        var hexCorners = new List<VertexId[]>();
        var hexEdgeIds = new List<List<EdgeId>>();

        for (int h = 0; h < hexes.Count; h++)
        {
            var hexId = new HexId(h);
            double cx = Math.Sqrt(3) * (hexes[h].Q + hexes[h].R / 2.0);
            double cy = 1.5 * hexes[h].R;

            var corners = new VertexId[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = Math.PI / 180.0 * (60 * i - 30);
                long kx = (long)Math.Round((cx + Math.Cos(angle)) * 1000);
                long ky = (long)Math.Round((cy + Math.Sin(angle)) * 1000);
                var key = (kx, ky);
                if (!vertexIds.TryGetValue(key, out var vertexId))
                {
                    vertexId = new VertexId(vertexIds.Count);
                    vertexIds[key] = vertexId;
                    vertexHexes[vertexId] = new HashSet<HexId>();
                    vertexEdges[vertexId] = new HashSet<EdgeId>();
                    vertexAdjacent[vertexId] = new HashSet<VertexId>();
                }

                corners[i] = vertexId;
                vertexHexes[vertexId].Add(hexId);
            }

            var edges = new List<EdgeId>(6);
            for (int i = 0; i < 6; i++)
            {
                var a = corners[i];
                var b = corners[(i + 1) % 6];
                var key = (Math.Min(a.Value, b.Value), Math.Max(a.Value, b.Value));
                if (!edgeIds.TryGetValue(key, out var edgeId))
                {
                    edgeId = new EdgeId(edgeIds.Count);
                    edgeIds[key] = edgeId;
                    edgeEnds[edgeId] = (a, b);
                    vertexEdges[a].Add(edgeId);
                    vertexEdges[b].Add(edgeId);
                    vertexAdjacent[a].Add(b);
                    vertexAdjacent[b].Add(a);
                }

                edges.Add(edgeId);
            }

            hexCorners.Add(corners);
            hexEdgeIds.Add(edges);
        }

        var builtEdges = edgeEnds
            .Select(e => new Edge(e.Key, e.Value.A, e.Value.B))
            .ToList();

        var builtVertices = vertexHexes.Keys
            .Select(v => new Vertex(v, vertexHexes[v].ToList(), vertexEdges[v].ToList(), vertexAdjacent[v].ToList()))
            .ToList();

        var builtHexes = new List<Hex>();
        int tokenIndex = 0;
        for (int h = 0; h < hexes.Count; h++)
        {
            var terrain = Terrains[h];
            int? token = terrain == TerrainKind.Desert ? null : NumberTokens[tokenIndex++];
            builtHexes.Add(new Hex(new HexId(h), terrain, token, hexCorners[h].ToList(), hexEdgeIds[h]));
        }

        return new Board(builtHexes, builtVertices, builtEdges);
    }
}
namespace Catan;

public static class StandardBoard
{
    private static readonly TerrainType[] TerrainTypes =
    {
        TerrainType.Desert,
        TerrainType.Forest, TerrainType.Forest, TerrainType.Forest, TerrainType.Forest,
        TerrainType.Fields, TerrainType.Fields, TerrainType.Fields, TerrainType.Fields,
        TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture, TerrainType.Pasture,
        TerrainType.Hills, TerrainType.Hills, TerrainType.Hills,
        TerrainType.Mountains, TerrainType.Mountains, TerrainType.Mountains
    };

    private static readonly int[] NumberTokenSequence =
    {
        5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
    };

    private static readonly (int Q, int R)[] Directions =
    {
        (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)
    };

    public static (HexGrid Grid, NumberLayout Numbers) Create()
    {
        var random = new Random();

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
        var hexIdAt = new Dictionary<(int Q, int R), HexId>();
        var terrainOf = new Dictionary<HexId, TerrainType>();
        var remainingTerrainTypes = TerrainTypes.ToList();
        for (int h = 0; h < hexes.Count; h++)
        {
            var terrainTypeIndex = random.Next(0, remainingTerrainTypes.Count);
            var terrainType = remainingTerrainTypes[terrainTypeIndex];
            remainingTerrainTypes.RemoveAt(terrainTypeIndex);

            var hexId = new HexId(h);
            var (Q, R) = hexes[h];
            hexIdAt[(Q, R)] = hexId;
            terrainOf[hexId] = terrainType;
            builtHexes.Add(new Hex(hexId, Q, R, terrainType, hexCorners[h].ToList(), hexEdgeIds[h]));
        }

        var tokens = new Dictionary<HexId, NumberToken>();
        int tokenIndex = 0;
        foreach (var hexId in SpiralFromCorner(hexIdAt, random.Next(0, Directions.Length)))
        {
            if (terrainOf[hexId] == TerrainType.Desert)
                continue;

            tokens[hexId] = new NumberToken(NumberTokenSequence[tokenIndex++]);
        }

        return (new HexGrid(builtHexes, builtVertices, builtEdges), new NumberLayout(tokens));
    }

    private static IEnumerable<HexId> SpiralFromCorner(
        IReadOnlyDictionary<(int Q, int R), HexId> hexIdAt,
        int corner)
    {
        for (int radius = 2; radius >= 1; radius--)
            foreach (var coord in Ring(radius, corner))
                yield return hexIdAt[coord];

        yield return hexIdAt[(0, 0)];
    }

    private static IEnumerable<(int Q, int R)> Ring(int radius, int corner)
    {
        int q = Directions[corner].Q * radius;
        int r = Directions[corner].R * radius;
        for (int side = 0; side < Directions.Length; side++)
        {
            var step = Directions[(corner + 2 + side) % Directions.Length];
            for (int i = 0; i < radius; i++)
            {
                yield return (q, r);
                q += step.Q;
                r += step.R;
            }
        }
    }
}
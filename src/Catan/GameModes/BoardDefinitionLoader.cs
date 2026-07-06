using System.Text.Json;
using System.Text.Json.Serialization;
using Catan.Economy;

namespace Catan.GameModes;

public sealed class BoardDefinitionLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters = { new JsonStringEnumConverter(), new ResourceTypeJsonConverter() }
    };

    public IReadOnlyList<BoardDefinition> LoadDirectory(string directory)
    {
        if (!Directory.Exists(directory))
            return [];

        return Directory
            .EnumerateFiles(directory, "*.json", SearchOption.TopDirectoryOnly)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .Select(Load)
            .ToList();
    }

    public BoardDefinition Load(string path) => Parse(File.ReadAllText(path), path);

    public BoardDefinition Parse(string json, string source)
    {
        BoardDefinition? definition;
        try
        {
            definition = JsonSerializer.Deserialize<BoardDefinition>(json, Options);
        }
        catch (JsonException ex)
        {
            throw new BoardDefinitionException($"'{source}' is not valid JSON: {ex.Message}", ex);
        }

        if (definition is null)
            throw new BoardDefinitionException($"'{source}' contains no board definition.");

        Validate(definition, source);
        return definition;
    }

    private static void Validate(BoardDefinition definition, string source)
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(definition.Name))
            errors.Add("'name' is required.");

        if (definition.MinPlayers < 1)
            errors.Add("'minPlayers' must be at least 1.");
        if (definition.MaxPlayers < definition.MinPlayers)
            errors.Add("'maxPlayers' must be greater than or equal to 'minPlayers'.");

        if (definition.Hexes.Count == 0)
            errors.Add("'hexes' must contain at least one hex.");

        var seen = new HashSet<(int, int)>();
        foreach (var hex in definition.Hexes)
        {
            if (!seen.Add((hex.Q, hex.R)))
                errors.Add($"duplicate hex at ({hex.Q}, {hex.R}).");

            if (hex.Terrain == TerrainType.Sea && hex.Token.HasValue)
                errors.Add($"sea hex at ({hex.Q}, {hex.R}) cannot carry a number token.");

            if (hex.Token is { } token and (< 2 or > 12 or 7))
                errors.Add($"hex at ({hex.Q}, {hex.R}) has invalid token {token} (must be 2-12 and not 7).");
        }

        int productiveCount = definition.Hexes.Count(h =>
            h.Terrain != TerrainType.Sea && TerrainYields.For(h.Terrain) != Yield.Nothing);
        int tokenCount = definition.Hexes.Count(h => h.Token.HasValue);
        if (tokenCount != productiveCount)
            errors.Add($"expected {productiveCount} number tokens (one per productive land hex) but found {tokenCount}.");

        foreach (var harbour in definition.Harbours)
        {
            if (harbour.Ratio < 2)
                errors.Add($"harbour at ({harbour.Edge.Q}, {harbour.Edge.R}, {harbour.Edge.Direction}) has invalid ratio {harbour.Ratio}.");
        }

        if (definition.Robber is { } robber && !seen.Contains((robber.Q, robber.R)))
            errors.Add($"'robber' at ({robber.Q}, {robber.R}) is not a defined hex.");
        if (definition.Pirate is { } pirate && !seen.Contains((pirate.Q, pirate.R)))
            errors.Add($"'pirate' at ({pirate.Q}, {pirate.R}) is not a defined hex.");

        if (errors.Count > 0)
            throw new BoardDefinitionException(
                $"'{source}' is not a valid board definition:{Environment.NewLine}  - {string.Join($"{Environment.NewLine}  - ", errors)}");
    }
}

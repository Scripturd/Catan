using Catan.Modding;

namespace Catan.Tests;

public sealed class BoardDefinitionLoaderTests
{
    private static readonly BoardDefinitionLoader Loader = new();

    [Fact]
    public void Parse_reads_a_valid_definition()
    {
        var definition = Loader.Parse("""
        {
          "name": "Valid",
          "minPlayers": 2,
          "maxPlayers": 4,
          "hexes": [ { "q": 0, "r": 0, "terrain": "Forest", "token": 6 } ]
        }
        """, "test");

        Assert.Equal("Valid", definition.Name);
        Assert.Single(definition.Hexes);
    }

    [Fact]
    public void Parse_rejects_malformed_json()
    {
        var ex = Assert.Throws<BoardDefinitionException>(() => Loader.Parse("{ not json", "test"));
        Assert.Contains("not valid JSON", ex.Message);
    }

    [Theory]
    [InlineData("""{ "name": "", "minPlayers": 3, "maxPlayers": 4, "hexes": [ { "q": 0, "r": 0, "terrain": "Desert" } ] }""")]
    [InlineData("""{ "name": "Dup", "minPlayers": 3, "maxPlayers": 4, "hexes": [ { "q": 0, "r": 0, "terrain": "Desert" }, { "q": 0, "r": 0, "terrain": "Desert" } ] }""")]
    [InlineData("""{ "name": "NoToken", "minPlayers": 3, "maxPlayers": 4, "hexes": [ { "q": 0, "r": 0, "terrain": "Forest" } ] }""")]
    [InlineData("""{ "name": "SeaToken", "minPlayers": 3, "maxPlayers": 4, "hexes": [ { "q": 0, "r": 0, "terrain": "Sea", "token": 6 } ] }""")]
    [InlineData("""{ "name": "Bad7", "minPlayers": 3, "maxPlayers": 4, "hexes": [ { "q": 0, "r": 0, "terrain": "Forest", "token": 7 } ] }""")]
    [InlineData("""{ "name": "Robber", "minPlayers": 3, "maxPlayers": 4, "hexes": [ { "q": 0, "r": 0, "terrain": "Desert" } ], "robber": { "q": 9, "r": 9 } }""")]
    [InlineData("""{ "name": "Players", "minPlayers": 4, "maxPlayers": 3, "hexes": [ { "q": 0, "r": 0, "terrain": "Desert" } ] }""")]
    public void Parse_rejects_invalid_definitions(string json)
    {
        Assert.Throws<BoardDefinitionException>(() => Loader.Parse(json, "test"));
    }

    [Fact]
    public void LoadDirectory_returns_empty_for_a_missing_directory()
    {
        Assert.Empty(Loader.LoadDirectory(Path.Combine(AppContext.BaseDirectory, "no-such-dir")));
    }
}

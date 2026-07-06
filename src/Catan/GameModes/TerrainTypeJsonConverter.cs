using System.Text.Json;
using System.Text.Json.Serialization;
using Catan.Board;

namespace Catan.GameModes;

internal sealed class TerrainTypeJsonConverter : JsonConverter<TerrainType>
{
    public override TerrainType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var name = reader.GetString();
        if (name is null)
            return null;

        return TerrainType.ByName(name) ?? throw new JsonException($"Unknown terrain '{name}'.");
    }

    public override void Write(Utf8JsonWriter writer, TerrainType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Name);
}

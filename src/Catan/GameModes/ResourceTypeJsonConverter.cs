using System.Text.Json;
using System.Text.Json.Serialization;
using Catan.Economy;

namespace Catan.GameModes;

internal sealed class ResourceTypeJsonConverter : JsonConverter<ResourceType>
{
    public override ResourceType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var name = reader.GetString();
        if (name is null)
            return null;

        return ResourceType.ByName(name) ?? throw new JsonException($"Unknown resource '{name}'.");
    }

    public override void Write(Utf8JsonWriter writer, ResourceType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Name);
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Civilization.Shared;

public class JsonPolymorphicConverter<TBase> : JsonConverter<TBase>
{
    private readonly Dictionary<string, Type> _typeMap;

    public JsonPolymorphicConverter()
    {
        _typeMap = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(TBase).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .ToDictionary(t => t.Name, t => t);
    }
    
    public override TBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        if (!doc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' field for polymorphic deserialization");

        var typeName = typeProp.GetString();
        if (typeName is null || !_typeMap.TryGetValue(typeName, out var derivedType))
            throw new JsonException($"Unknown type discriminator: '{typeName}'");

        var json = doc.RootElement.GetRawText();
        return (TBase?)JsonSerializer.Deserialize(json, derivedType, options);
    }
    
    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), options));

        writer.WriteStartObject();
        writer.WriteString("type", value.GetType().Name);

        foreach (var prop in jsonDoc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
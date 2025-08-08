using System.Text.Json;
using System.Text.Json.Serialization;
using Civilization.Shared.Commands;
using Civilization.Shared.Packets.ServerMessages;

namespace Civilization.Shared;

public static class SharedJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new JsonPolymorphicConverter<BaseCommand>(),
            new JsonPolymorphicConverter<BaseServerMessage>(),
        },
        WriteIndented = false
    };
}
using System.Text.Json;
using System.Text.Json.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.GamesDatabase;

/// <summary>
/// Custom JSON converter for <see cref="GameProfile"/> objects.
/// Handles conversion between JSON and game registry entries.
/// </summary>
public class GameRegistryJsonConverter : JsonConverter<GameProfile>
{
    /// <summary>
    /// Reads and converts the JSON to a <see cref="GameProfile"/> object.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type of object to convert.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>The converted <see cref="GameProfile"/> object.</returns>
    public override GameProfile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement obj = doc.RootElement;

        // Extract basic game information from JSON.
        string name = obj.GetProperty("name").GetString() ?? string.Empty;
        string description = obj.GetProperty("description").GetString() ?? string.Empty;
        string blowfishKey = obj.GetProperty("blowfishKey").GetString() ?? string.Empty;
        bool isTtarch2 = obj.GetProperty("isTtarch2").GetBoolean();
        int ttarchVersion = obj.GetProperty("ttarchVersion").GetInt32();
        string luaVersion = obj.GetProperty("luaVersion").GetString() ?? string.Empty;
        string metaStreamVersion = obj.GetProperty("metaStreamVersion").GetString() ?? string.Empty;
        bool enableOodleCompression = obj.GetProperty("enableOodleCompression").GetBoolean();
        var areSymbolsHashed = true;
        if (obj.TryGetProperty("areSymbolsHashed", out JsonElement value))
        {
            if (value.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                areSymbolsHashed = value.GetBoolean();
            }
            else if (value.ValueKind == JsonValueKind.String &&
                     bool.TryParse(value.GetString(), out bool parsed))
            {
                areSymbolsHashed = parsed;
            }
        }
      
        // Attempt to resolve the blowfish key from known enum values. If not, use the string as a key.
        if (Enum.TryParse(blowfishKey, out T3BlowfishKey enumBlowfishKey))
        {
            blowfishKey = enumBlowfishKey.GetBlowfishKey();
        }

        return new GameProfile
        {
            Name = name,
            Description = description,
            BlowfishKey = blowfishKey,
            IsTtarch2 = isTtarch2,
            TtarchVersion = ttarchVersion.TtarchVersionFromNumber(isTtarch2),
            LuaVersion = luaVersion.ParseLuaVersion(),
            MetaStreamVersion = metaStreamVersion.StreamVersionFromString(),
            EnableOodleCompression = enableOodleCompression,
            AreSymbolsHashed = areSymbolsHashed
        };
    }

    /// <summary>
    /// Writes a <see cref="GameProfile"/> object as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The <see cref="GameProfile"/> value to write.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(Utf8JsonWriter writer, GameProfile value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteString("description", value.Description);
        writer.WriteString("blowfishKey", value.BlowfishKey);
        writer.WriteBoolean("isTtarch2", value.IsTtarch2);
        writer.WriteNumber("ttarchVersion", value.TtarchVersion.ToJsonNumber());
        writer.WriteString("luaVersion", value.LuaVersion.ToVersionString());
        writer.WriteString("metaStreamVersion", value.MetaStreamVersion.ToJsonString());
        writer.WriteBoolean("enableOodleCompression", value.EnableOodleCompression);
        writer.WriteEndObject();
    }
}
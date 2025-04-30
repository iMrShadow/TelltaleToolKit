using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility;

namespace TelltaleToolKit.GamesDatabase;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Custom JSON converter for <see cref="GameDescriptor"/> objects.
/// Handles conversion between JSON and game registry entries.
/// </summary>
public class GameRegistryJsonConverter : JsonConverter<GameDescriptor>
{
    /// <summary>
    /// Reads and converts the JSON to a <see cref="GameDescriptor"/> object.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type of object to convert.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>The converted <see cref="GameDescriptor"/> object.</returns>
    public override GameDescriptor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

        // Attempt to resolve the blowfish key from known enum values. If not, use the string as a key.
        if (Enum.TryParse(blowfishKey, out T3BlowfishKey myStatus))
        {
            blowfishKey = myStatus.ToString();
        }

        // Create and return a new GameDescriptor instance with the extracted properties.
        return new GameDescriptor
        {
            Name = name,
            Description = description,
            BlowfishKey = blowfishKey,
            IsTtarch2 = isTtarch2,
            TtarchVersion = ttarchVersion.TtarchVersionFromNumber(isTtarch2),
            LuaVersion = luaVersion.ParseLuaVersion(),
            MetaStreamVersion = metaStreamVersion.StreamVersionFromString(),
            EnableOodleCompression = enableOodleCompression
        };
    }

    /// <summary>
    /// Writes a <see cref="GameDescriptor"/> object as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The <see cref="GameDescriptor"/> value to write.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(Utf8JsonWriter writer, GameDescriptor value, JsonSerializerOptions options)
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
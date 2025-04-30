using System.Text.Json;
using System.Text.Json.Serialization;

namespace TelltaleToolKit.Reflection;

/// <summary>
/// Provides custom serialization and deserialization for <see cref="MetaClass"/> objects.
/// Handles conversion between JSON and strongly-typed reflection metadata.
/// </summary>
public class MetaClassJsonConverter : JsonConverter<MetaClass>
{
    /// <summary>
    /// Reads a <see cref="MetaClass"/> from JSON.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> positioned at the start of the object.</param>
    /// <param name="typeToConvert">The type being converted (should be <see cref="MetaClass"/>).</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>The converted <see cref="MetaClass"/> object.</returns>
    /// <exception cref="Exception">Thrown if the type property is missing or invalid.</exception>
    public override MetaClass Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement obj = doc.RootElement;

        // Read the class type name and resolve to MetaClassType
        string typeName = obj.GetProperty("type").GetString()
                          ?? throw new InvalidDataException("Invalid MetaClass type!");
        MetaClassType classType = MetaClassTypeRegistry.GetByName(typeName);

        // Read CRC32 value
        uint crc32 = obj.GetProperty("crc32").GetUInt32();

        // TODO: Check if the class already exists.
        // If it does, skip reading the members
        // This is a minor optimization.

        // Read members
        List<MetaMember> members = [];

        if (obj.TryGetProperty("members", out JsonElement propsElement) &&
            propsElement.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement propObj in propsElement.EnumerateArray())
            {
                // Member name
                string name = propObj.GetProperty("name").GetString()
                              ?? throw new InvalidDataException("Missing property name in MetaMember.");

                // Member type
                string memberTypeName = propObj.GetProperty("type").GetString()
                                        ?? throw new InvalidDataException("Missing property type in MetaMember.");
                MetaClassType memberType = MetaClassTypeRegistry.GetByName(memberTypeName);

                // Member flags
                int flags = propObj.GetProperty("flags").GetInt32();

                members.Add(new MetaMember(name, memberType, (MetaFlags)flags));
            }
        }

        return new MetaClass { ClassType = classType, Crc32 = crc32, Members = members };
    }

    /// <summary>
    /// Writes a <see cref="MetaClass"/> object as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The <see cref="MetaClass"/> value to write.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(Utf8JsonWriter writer, MetaClass value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.ClassType.Symbol.SymbolName);
        writer.WriteNumber("crc32", value.Crc32);
        writer.WriteStartArray("members");
        foreach (MetaMember prop in value.Members)
        {
            writer.WriteStartObject();
            writer.WriteString("name", prop.MemberName);
            writer.WriteString("type", prop.Type.Symbol.SymbolName);
            writer.WriteNumber("flags", (int)prop.Flags);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
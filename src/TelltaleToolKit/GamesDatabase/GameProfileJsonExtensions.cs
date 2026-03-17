using System.Collections.Immutable;
using System.Text.Json;
using TelltaleToolKit.Reflection;

namespace TelltaleToolKit.GamesDatabase;

public class GameProfileJsonExtensions
{
    /// <summary>
    /// Serializes GameProfile.Classes to JSON.
    /// </summary>
    public static string ToClassesJson(GameProfile profile, JsonSerializerOptions options)
    {
        ImmutableSortedDictionary<string, uint> classes = profile.Classes.ToDictionary(
            kvp => kvp.Key.Symbol.SymbolName,
            kvp => kvp.Value
        ).ToImmutableSortedDictionary();

        return JsonSerializer.Serialize(classes, options);
    }
    
    /// <summary>
    /// Reads JSON produced by <see cref="ToClassesJson"/> and applies it back onto GameProfile.Classes.
    /// </summary>
    public static void ReadClassesJsonInto(GameProfile profile, string json, JsonSerializerOptions options)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, uint>>(json, options);
        if (dict is null)
            return;

        profile.Classes.Clear();

        foreach ((string key, uint crc32) in dict)
        {
            MetaClassType type = MetaClassTypeRegistry.GetByName(key);
            profile.Classes[type] = crc32;
        }
    }
}
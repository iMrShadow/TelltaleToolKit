using System.Text.Json;
using TelltaleToolKit.Meta;

namespace TelltaleToolKit.Games;

public class GameProfileJsonExtensions
{
    /// <summary>
    ///     Serializes GameProfile.Classes to JSON.
    /// </summary>
    public static string ToClassesJson(GameProfile profile, JsonSerializerOptions options)
    {
        SortedDictionary<string?, uint> classes = new(
            profile.Classes.ToDictionary(
                kvp => kvp.Key.Symbol.DebugString,
                kvp => kvp.Value
            )
        );

        return JsonSerializer.Serialize(classes, options);
    }

    /// <summary>
    ///     Reads JSON produced by <see cref="ToClassesJson" /> and applies it back onto GameProfile.Classes.
    /// </summary>
    public static void ReadClassesJsonInto(GameProfile profile, string json, JsonSerializerOptions options)
    {
        Dictionary<string, uint>? dict = JsonSerializer.Deserialize<Dictionary<string, uint>>(json, options);
        if (dict is null)
        {
            return;
        }

        profile.Classes.Clear();

        foreach ((string key, uint crc32) in dict)
        {
            MetaClassType type = MetaClassTypeRegistry.GetByName(key);
            profile.Classes[type] = crc32;
        }
    }
}

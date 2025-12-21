using System.Text.Encodings.Web;
using System.Text.Json;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit;

public class GameContext
{
    public string GameName { get; }
    public GameDescriptor Descriptor;
    public MetaStreamConfiguration DefaultMetaStreamConfig { get; }

    // Archive management
    private List<ArchiveBase> _loadedArchives = new();
    
    public GameContext(GameDescriptor descriptor)
    {
      
        Descriptor = descriptor;
        GameName = descriptor.Name;
        DefaultMetaStreamConfig = new MetaStreamConfiguration
        {
            AreSymbolsHashed = descriptor.AreSymbolsHashed,
            Version = descriptor.MetaStreamVersion
        };
    }
    // public HashDatabase.HashDatabase? GameSpecificDatabase { get; set; }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() }
    };

    /// <summary>
    /// Gets the default <see cref="MetaStreamConfiguration"/> for the currently active game.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public MetaStreamConfiguration DefaultMetaStreamConfiguration => new()
    {
        AreSymbolsHashed = Descriptor.AreSymbolsHashed,
        Version = Descriptor.MetaStreamVersion
    };

    public MetaClass? GetMetaClassDescription(Type? type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        KeyValuePair<MetaClassType, uint>? match =
            Descriptor.Classes.FirstOrDefault(tc => tc.Key.LinkingType == type);

        return T3Kit.Instance.GetClass(match.Value.Key.Symbol, match.Value.Value);
    }

    public MetaClass? GetMetaClassDescription(Symbol? symbol)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        KeyValuePair<MetaClassType, uint>? match = Descriptor.Classes
            .FirstOrDefault(tc => tc.Key.Symbol.Crc64 == symbol.Crc64);

        return T3Kit.Instance.GetClass(match.Value.Key.Symbol, match.Value.Value);
    }

    public bool IsMetaClassDescriptionRegistered(MetaClass? desc)
        => desc is not null && Descriptor.Classes.ContainsKey(desc.ClassType);


    // public void PrintRegisteredClasses()
    // {
    //     MetaClassRegistry.PrintRegisteredClasses();
    // }

    private MetaClass? GetMetaClassDescription(MetaClassType type)
    {
        if (!Descriptor.Classes.ContainsKey(type))
        {
            Console.WriteLine($"Game descriptor doesn't have description for {type.FullTypeName}");
            return null;
        }

        if (Descriptor.Classes.TryGetValue(type, out uint crc32))
        {
            return T3Kit.Instance.GetClass(type.Symbol, crc32);
        }

        Console.WriteLine($"Game descriptor doesn't have description for {type.FullTypeName} with crc32 {crc32}!");
        return null;
    }

    /// <summary>
    /// Loads and parses a Telltale archive file (.ttarch or .ttarch2) using the specified blowfish key for decryption.
    /// </summary>
    /// <param name="ttarch">The path to the archive file.</param>
    /// <param name="game">The blowfish key for the game.</param>
    /// <param name="sort">Whether to sort archive entries.</param>
    /// <param name="debugPrint">Whether to print debug information during loading.</param>
    /// <returns>An <see cref="ArchiveBase"/> representing the loaded archive.</returns>
    /// <exception cref="NotSupportedException">Thrown if the archive type is unsupported.</exception>
    public ArchiveBase Load(string ttarch, bool sort = true,
        bool debugPrint = false)
    {
        return T3Kit.Instance.Load(ttarch, Descriptor.BlowfishKey, sort, debugPrint);
    }

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified stream.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="stream">The stream to write to.</param>
    public void Save<T>(T obj, Stream stream) where T : class, new()
        => Save(obj, stream, DefaultMetaStreamConfiguration);

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified file using a provided <see cref="MetaStreamConfiguration"/>.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="fileName">The path to the file to save to.</param>
    /// <param name="configuration">The <see cref="MetaStreamConfiguration"/> to use during serialization.</param>
    public void Save<T>(T obj, string fileName, MetaStreamConfiguration configuration) where T : class, new() =>
        Save(obj, File.OpenWrite(fileName), configuration);

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified stream using a provided <see cref="MetaStreamConfiguration"/>.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="configuration">The <see cref="MetaStreamConfiguration"/> to use during serialization.</param>
    public static void Save<T>(T obj, Stream stream, MetaStreamConfiguration configuration) where T : class, new()
    {
        var streamWriter = new MetaStreamWriter(stream, configuration);
        streamWriter.Serialize(ref obj);
        streamWriter.Save();
    }

    public string GetBlowfishKey()
    {
        return Descriptor.BlowfishKey;
    }
}
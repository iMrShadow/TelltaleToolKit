using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility;

namespace TelltaleToolKit;

public class TTKContext
{
    private GameDescriptor _gameDescriptor;
    public TTKContext(GameDescriptor  gameDescriptor)
    {
        _gameDescriptor = gameDescriptor;
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
        AreSymbolsHashed = _gameDescriptor.AreSymbolsHashed,
        Version = _gameDescriptor.MetaStreamVersion
    };

    public MetaClass? GetMetaClassDescription(Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);
        
        KeyValuePair<MetaClassType, uint>? match =
            _gameDescriptor.Classes.FirstOrDefault(tc => tc.Key.LinkingType == type);

        return TTKGlobalContext.Instance().GetClass(match.Value.Key.Symbol, match.Value.Value);
    }

    public MetaClass? GetMetaClassDescription(Symbol? symbol)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        KeyValuePair<MetaClassType, uint>? match = _gameDescriptor.Classes
            .FirstOrDefault(tc => tc.Key.Symbol.Crc64 == symbol.Crc64);

        return TTKGlobalContext.Instance().GetClass(match.Value.Key.Symbol, match.Value.Value);
    }
    
    public bool IsMetaClassDescriptionRegistered(MetaClass? desc)
        => desc is not null && _gameDescriptor.Classes.ContainsKey(desc.ClassType);
    

    // public void PrintRegisteredClasses()
    // {
    //     MetaClassRegistry.PrintRegisteredClasses();
    // }

    private MetaClass? GetMetaClassDescription(MetaClassType type)
    {
        if (!_gameDescriptor.Classes.ContainsKey(type))
        {
            Console.WriteLine($"Game descriptor doesn't have description for {type.FullTypeName}");
            return null;
        }
        
        if (_gameDescriptor.Classes.TryGetValue(type, out uint crc32))
        {
            return TTKGlobalContext.Instance().GetClass(type.Symbol, crc32);
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
       return TTKGlobalContext.Instance().Load(ttarch, _gameDescriptor.BlowfishKey, sort, debugPrint);
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
}
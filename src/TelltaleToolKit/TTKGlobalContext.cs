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

public class TTKGlobalContext
{
    private static TTKGlobalContext? _instance;

    private MetaClassRegistry MetaClassRegistry { get; set; }
    private MetaClassSerializerSelector MetaClassSerializerSelector { get; set; } = new();

    private TTKGlobalContext()
    {
        MetaClassRegistry = new MetaClassRegistry();
    }

    private List<GameDescriptor> RegisteredGames { get; set; } = [];
    public static TTKGlobalContext Instance() => _instance ??= new TTKGlobalContext();

    public HashDatabase.HashDatabase? MainHashDatabase { get; set; }

    public void Register(IEnumerable<MetaClass> metaClassDescriptions)
    {
        MetaClassRegistry.Register(metaClassDescriptions);
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() }
    };

    // public void LoadMetaClassDescriptionsFromJsonFolder(string folderPath)
    // {
    //     foreach (string filePath in Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories))
    //     {
    //         LoadMetaClassDescriptionsFromJson(File.ReadAllText(filePath));
    //     }
    // }
    //
    //
    // public void LoadMetaClassDescriptionsFromJsonFile(string path)
    // {
    //     LoadMetaClassDescriptionsFromJson(File.ReadAllText(path));
    // }

    public List<MetaClass>? GetMetaClassDescriptionsFromJson(string json)
    {
        return JsonSerializer.Deserialize<List<MetaClass>>(json, JsonOptions);
    }

    public GameDescriptor GetTelltaleGame(string gameName)
    {
        ArgumentNullException.ThrowIfNull(gameName, nameof(gameName));

        return RegisteredGames.First(g => g.Name.Equals(gameName, StringComparison.OrdinalIgnoreCase));
    }

    public GameDescriptor FindGame(string gameName)
    {
        ArgumentNullException.ThrowIfNull(gameName, nameof(gameName));
        return RegisteredGames.First(g => g.Id.Equals(gameName, StringComparison.OrdinalIgnoreCase));
    }

    public void RegisterGame(GameDescriptor gameDescriptor)
    {
        ArgumentNullException.ThrowIfNull(gameDescriptor, nameof(gameDescriptor));
        RegisteredGames.Add(gameDescriptor);
    }

    public void RegisterGames(IEnumerable<GameDescriptor> games)
        => RegisteredGames.AddRange(games);

    public MetaClassSerializer<T> GetSerializer<T>() where T : new()
        => MetaClassSerializerSelector.GetSerializer<T>();

    public MetaClassSerializer GetSerializer(Type type)
        => MetaClassSerializerSelector.GetSerializer(type);

    // Utility to sanitize file names
    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }

    public void RegisterClass(MetaClass metaClass)
    {
        MetaClassRegistry.RegisterClass(metaClass);
    }

    public MetaClass? GetClass(Symbol typeSymbol, uint crc32)
        => MetaClassRegistry.GetClass(typeSymbol, crc32);

    public void PrintRegisteredClasses()
    {
        MetaClassRegistry.PrintRegisteredClasses();
    }

    public void Load(string dataFolder)
    {
        string gameDescriptorsFolder = Path.Join(dataFolder, "game_descriptors");

        string[] files = Directory.GetFiles(gameDescriptorsFolder, "*.json", SearchOption.TopDirectoryOnly);

        string versionDbFolder = Path.Join(dataFolder, "versiondb"); // Unused

        // Just asynchronously load all database files. It is thread safe.
        Parallel.ForEach(files, filePath =>
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string json = File.ReadAllText(filePath);
            var gameRegistry = JsonSerializer.Deserialize<GameDescriptor>(json, JsonOptions);

            if (gameRegistry == null)
                return;

            gameRegistry.Id = fileName;
            lock (RegisteredGames)
            {
                RegisterGame(gameRegistry);
            }

            // Look for corresponding snapshot file
            string snapshotPath = Path.Combine(versionDbFolder, fileName + ".vdb.json");
            if (File.Exists(snapshotPath))
            {
                string snapshotJson = File.ReadAllText(snapshotPath);
                List<MetaClass>? metaClassDescriptions = GetMetaClassDescriptionsFromJson(snapshotJson);

                if (metaClassDescriptions is null)
                {
                    return;
                }

                lock (MetaClassRegistry)
                {
                    Register(metaClassDescriptions);
                }

                foreach (MetaClass metaClassDescription in metaClassDescriptions)
                {
                    lock (gameRegistry.Classes)
                    {
                        gameRegistry.Classes[metaClassDescription.ClassType] = metaClassDescription.Crc32;
                    }
                }
            }
        });

        string hashDb = Path.Join(dataFolder, "hashdb", "hash.db");

        MainHashDatabase = new HashDatabase.HashDatabase(hashDb);
        MainHashDatabase.ImportFromDirectory(Path.Join(dataFolder, "hashdb"));
    }

    public static void PrintRegisteredTypes()
    {
        MetaClassTypeRegistry.PrintRegisteredTypes();
    }

    // Dump all metaclassdescriptions for all registered games to a folder
    private void DumpAllGamesMetaClassDescriptions(string folderPath)
    {
        Directory.CreateDirectory(folderPath);

        foreach (GameDescriptor game in RegisteredGames)
        {
            DumpGameMetaClassDescriptions(game, Path.Combine(folderPath, $"{SanitizeFileName(game.Name)}.json"));
        }
    }

    // Dump metaclassdescriptions for a single game
    private void DumpGameMetaClassDescriptions(GameDescriptor game, string filePath)
    {
        if (game == null)
            throw new ArgumentNullException(nameof(game));

        var metaClassDescriptions = new List<MetaClass>();
        foreach (KeyValuePair<MetaClassType, uint> entry in game.Classes)
        {
            MetaClass? desc = MetaClassRegistry.GetClass(entry.Key, entry.Value);
            if (desc != null)
                metaClassDescriptions.Add(desc);
        }

        string json = JsonSerializer.Serialize(metaClassDescriptions, JsonOptions);
        File.WriteAllText(filePath, json, Encoding.ASCII);
    }

    private void DumpAllMetaClassDescriptions(string path)
    {
        string json = JsonSerializer.Serialize(MetaClassRegistry.Classes.Values, JsonOptions);
        File.WriteAllText(path, json, Encoding.ASCII);
    }

    public static void DumpMetaClassDescriptions(IEnumerable<MetaClass> metaClassDescriptions, string path)
    {
        string json = JsonSerializer.Serialize(metaClassDescriptions, JsonOptions);
        File.WriteAllText(path, json, Encoding.ASCII);
    }

    private MetaClass? GetMetaClassDescription(MetaClassType type, uint crc32)
    {
        return MetaClassRegistry.GetClass(type, crc32);
    }

    public void ResolveSymbol(Symbol symbol)
    {
        MainHashDatabase?.ResolveSymbol(symbol);

        if (!symbol.HasString() && Symbols.TryGetValue(symbol.Crc64, out string? value))
        {
            symbol.SymbolName = value;
        }
    }

    public void ResolveSymbols(List<Symbol> symbols)
    {
        MainHashDatabase?.ResolveSymbols(symbols);

        foreach (Symbol symbol in symbols.Where(symbol => !symbol.HasString()))
        {
            ResolveSymbol(symbol);
        }
    }

    private readonly Dictionary<ulong, string> Symbols = [];

    public void InsertSymbolsIntoDatabase(List<Symbol> symbols)
    {
        foreach (Symbol symbol in symbols)
        {
            Symbols.TryAdd(symbol.Crc64, symbol.SymbolName);
        }
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
    public ArchiveBase Load(string ttarch, T3BlowfishKey game, bool sort = true,
        bool debugPrint = false)
    {
        if (ttarch.EndsWith(".ttarch2"))
        {
            return ArchiveBase.Load<T3Archive2>(ttarch, game, sort, debugPrint);
        }

        if (ttarch.EndsWith(".ttarch"))
        {
            return ArchiveBase.Load<T3Archive>(ttarch, game, sort, debugPrint);
        }

        throw new NotSupportedException($"Unsupported archive type: {ttarch}");
    }

    /// <summary>
    /// Loads and parses a Telltale archive file (.ttarch or .ttarch2) using the specified blowfish key for decryption.
    /// </summary>
    /// <param name="ttarch">The path to the archive file.</param>
    /// <param name="key">The blowfish key for the game.</param>
    /// <param name="sort">Whether to sort archive entries.</param>
    /// <param name="debugPrint">Whether to print debug information during loading.</param>
    /// <returns>An <see cref="ArchiveBase"/> representing the loaded archive.</returns>
    /// <exception cref="NotSupportedException">Thrown if the archive type is unsupported.</exception>
    public ArchiveBase Load(string ttarch, string key, bool sort = true,
        bool debugPrint = false)
    {
        if (ttarch.EndsWith(".ttarch2"))
        {
            return ArchiveBase.Load<T3Archive2>(ttarch, key, sort, debugPrint);
        }

        if (ttarch.EndsWith(".ttarch"))
        {
            return ArchiveBase.Load<T3Archive>(ttarch, key, sort, debugPrint);
        }

        throw new NotSupportedException($"Unsupported archive type: {ttarch}");
    }
}
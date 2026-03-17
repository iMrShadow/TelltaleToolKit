using System.Text;
using System.Text.Json;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit;

public class Toolkit
{
    private static Toolkit? _instance;
    private readonly Dictionary<string, Workspace> _workspaces = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, GameProfile> _gameProfiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly JsonSerializerOptions _jsonOptions;

    public static bool IsInitialized => _instance != null;

    private Toolkit(Configuration config)
    {
        Config = config ?? throw new ArgumentNullException(nameof(config));

        // Initialize components
        ClassRegistry = new MetaClassRegistry();
        SerializerSelector = new MetaClassSerializerSelector();
        GlobalHashDatabase = new HashDatabase.HashDatabase();

        // Setup JSON options
        _jsonOptions = config.JsonOptions ?? new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() },
            WriteIndented = true
        };

        if (!string.IsNullOrEmpty(config.DataFolder))
        {
            LoadMetaClassDescriptions();
            LoadGameProfiles(config.DataFolder);
            LoadHashDatabase(config.DataFolder);
        }
    }

    /// <summary>
    /// Gets the singleton instance of TelltaleToolkit.
    /// Must be initialized first using <see cref="Initialize"/>.
    /// </summary>
    public static Toolkit Instance
    {
        get
        {
            if (_instance == null)
                throw new InvalidOperationException(
                    "TelltaleToolkit must be initialized first. Call TelltaleToolkit.Initialize()");
            return _instance;
        }
    }

    /// <summary>
    /// Gets the configuration used to initialize the toolkit.
    /// </summary>
    public Configuration Config { get; private set; }

    /// <summary>
    /// Gets the global MetaClass registry.
    /// </summary>
    public MetaClassRegistry ClassRegistry { get; private set; }

    /// <summary>
    /// Gets the global hash database for symbol resolution.
    /// </summary>
    public HashDatabase.HashDatabase GlobalHashDatabase { get; private set; }

    /// <summary>
    /// Gets the serializer selector for object serialization.
    /// </summary>
    public MetaClassSerializerSelector SerializerSelector { get; private set; }

    /// <summary>
    /// Gets all registered game profiles.
    /// </summary>
    public IReadOnlyDictionary<string, GameProfile> GameProfiles => _gameProfiles;

    /// <summary>
    /// Initializes the TelltaleToolkit with the specified configuration.
    /// Must be called once before using the toolkit.
    /// </summary>
    public static void Initialize(Configuration config)
    {
        if (_instance != null)
            throw new InvalidOperationException("TelltaleToolkit is already initialized");

        _instance = new Toolkit(config);
    }

    /// <summary>
    /// Creates a game workspace for the specified game.
    /// </summary>
    public Workspace CreateWorkspace(string workspaceName, string gameProfile)
    {
        if (!_gameProfiles.TryGetValue(gameProfile, out GameProfile? profile))
            throw new KeyNotFoundException($"Game profile '{gameProfile}' not found");

        var workspace = new Workspace(workspaceName, this, profile);
        _workspaces[workspaceName] = workspace;
        return workspace;
    }

    /// <summary>
    /// Gets an existing workspace, or creates one if it doesn't exist.
    /// </summary>
    public Workspace? GetWorkspace(string gameProfileName)
    {
        return _workspaces.GetValueOrDefault(gameProfileName);
    }

    /// <summary>
    /// Registers a game profile.
    /// </summary>
    public void RegisterGameProfile(GameProfile profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));

        _gameProfiles[profile.Name] = profile;

        // Load associated meta class descriptions
        if (!string.IsNullOrEmpty(Config.DataFolder))
        {
            LoadMetaClassDescriptionsForGame(profile);
        }
    }

    /// <summary>
    /// Resolves a symbol using the configured hash resolution strategy.
    /// </summary>
    public bool ResolveSymbol(Symbol symbol)
    {
        if (symbol == null)
            throw new ArgumentNullException(nameof(symbol));

        return GlobalHashDatabase.ResolveSymbol(symbol);
    }

    /// <summary>
    /// Resolves multiple symbols in batch.
    /// </summary>
    public void ResolveSymbols(IEnumerable<Symbol> symbols)
    {
        foreach (Symbol? symbol in symbols)
        {
            ResolveSymbol(symbol);
        }
    }

    /// <summary>
    /// Loads an object from a file using the specified configuration.
    /// </summary>
    public T LoadObject<T>(string fileName, out MetaStreamConfiguration config) where T : class, new()
    {
        using FileStream stream = File.OpenRead(fileName);
        return LoadObject<T>(stream, out config);
    }

    /// <summary>
    /// Loads an object from a stream using the specified configuration.
    /// </summary>
    public T LoadObject<T>(Stream stream, out MetaStreamConfiguration config) where T : class, new()
    {
        var reader = new MetaStreamReader(stream);
        var obj = default(T);
        reader.PreSerialize(ref obj);
        reader.Serialize(ref obj);

        if (obj == null)
            throw new InvalidOperationException($"Failed to deserialize object of type {typeof(T).Name}");

        config = reader.Configuration;
        return obj;
    }

    /// <summary>
    /// Loads an object from a file using the specified configuration.
    /// </summary>
    public object LoadObject(string fileName, out MetaStreamConfiguration config)
    {
        using FileStream stream = File.OpenRead(fileName);
        return LoadObject(stream, out config);
    }

    /// <summary>
    /// Loads an object from a stream using the specified configuration.
    /// </summary>
    public object LoadObject(Stream stream, out MetaStreamConfiguration config)
    {
        var reader = new MetaStreamReader(stream);
        Type type = reader.Configuration.SerializedClasses.First().ClassType.LinkingType;

        object? obj = Activator.CreateInstance(type);

        MetaClassSerializer serializer = Instance.GetSerializer(obj.GetType());
        serializer.PreSerialize(ref obj, reader);
        serializer.Serialize(ref obj, reader);

        if (obj == null)
            throw new InvalidOperationException($"Failed to deserialize object of type {type.Name}");

        config = reader.Configuration;
        return obj;
    }

    /// <summary>
    /// Saves an object to a file using the specified configuration.
    /// </summary>
    public void SaveObject<T>(T obj, string fileName, MetaStreamConfiguration config) where T : class, new()
    {
        using FileStream stream = File.OpenWrite(fileName);
        SaveObject(obj, stream, config);
    }

    /// <summary>
    /// Saves an object to a stream using the specified configuration.
    /// </summary>
    public void SaveObject<T>(T obj, Stream stream, MetaStreamConfiguration config) where T : class, new()
    {
        var writer = new MetaStreamWriter(stream, config);
        T refObj = obj;
        writer.PreSerialize(ref refObj);
        writer.Serialize(ref refObj);
        writer.Save();
    }


    /// <summary>
    /// Saves an object to a file using the specified configuration.
    /// </summary>
    public void SaveObject(object obj, string fileName, MetaStreamConfiguration config)
    {
        using FileStream stream = File.OpenWrite(fileName);
        SaveObject(obj, stream, config);
    }

    /// <summary>
    /// Saves an object to a stream using the specified configuration.
    /// </summary>
    public void SaveObject(object obj, Stream stream, MetaStreamConfiguration config)
    {
        var writer = new MetaStreamWriter(stream, config);

        MetaClassSerializer serializer = Instance.GetSerializer(obj.GetType());
        serializer.PreSerialize(ref obj, writer);
        serializer.Serialize(ref obj, writer);
        writer.Save();
    }

    /// <summary>
    /// Loads an archive with the specified blowfish key.
    /// </summary>
    public ArchiveBase LoadArchive(string archivePath, string blowfishKey, bool sort = true, bool debugPrint = false)
    {
        if (archivePath.EndsWith(".ttarch2", StringComparison.OrdinalIgnoreCase))
        {
            return ArchiveBase.Load<T3Archive2>(archivePath, blowfishKey, sort, debugPrint);
        }

        if (archivePath.EndsWith(".ttarch", StringComparison.OrdinalIgnoreCase))
        {
            return ArchiveBase.Load<T3Archive>(archivePath, blowfishKey, sort, debugPrint);
        }

        throw new NotSupportedException($"Unsupported archive format: {Path.GetExtension(archivePath)}");
    }

    private void LoadGameProfiles(string dataFolder)
    {
        string profilesPath = Path.Combine(dataFolder, "game_profiles");
        if (!Directory.Exists(profilesPath))
            return;

        string[] files = Directory.GetFiles(profilesPath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                var gameProfile = JsonSerializer.Deserialize<GameProfile>(json, _jsonOptions);

                if (gameProfile != null)
                {
                    gameProfile.Id = Path.GetFileNameWithoutExtension(file);
                    RegisterGameProfile(gameProfile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load game profile from {file}: {ex.Message}");
            }
        }
    }

    private void LoadHashDatabase(string dataFolder)
    {
        if (string.IsNullOrEmpty(dataFolder))
            return;

        string hashDbDir = Path.Combine(dataFolder, "hashdb");
        if (!Directory.Exists(hashDbDir))
            return;

        try
        {
            // Load global symbols
            GlobalHashDatabase.ImportFromDirectory(hashDbDir, recursive: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load hash databases: {ex.Message}");
        }
    }

    private void LoadMetaClassDescriptionsForGame(GameProfile profile)
    {
        if (string.IsNullOrEmpty(Config.DataFolder))
            return;

        string snapshotPath = Path.Combine(Config.DataFolder, "versiondb", $"{profile.Id}.vdb.json");
        if (!File.Exists(snapshotPath))
            return;

        try
        {
            string json = File.ReadAllText(snapshotPath);
            GameProfileJsonExtensions.ReadClassesJsonInto(profile, json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load meta class descriptions for {profile.Id}: {ex.Message}");
        }
    }

    private void LoadMetaClassDescriptions()
    {
        if (string.IsNullOrEmpty(Config.DataFolder))
            return;

        string globalDbPath = Path.Combine(Config.DataFolder, "versiondb", $"global.vdb.json");

        if (!File.Exists(globalDbPath))
            return;

        string json = File.ReadAllText(globalDbPath);
        var metaClasses = JsonSerializer.Deserialize<List<MetaClass>>(json, _jsonOptions);

        if (metaClasses != null)
        {
            ClassRegistry.Register(metaClasses);
        }
    }

    public MetaClassSerializer<T> GetSerializer<T>() where T : new()
        => SerializerSelector.GetSerializer<T>();

    public MetaClassSerializer GetSerializer(Type type)
        => SerializerSelector.GetSerializer(type);

    // Utility to sanitize file names
    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }

    public void DumpMetaClassDescriptions(IEnumerable<MetaClass> metaClassDescriptions, string path)
    {
        string json = JsonSerializer.Serialize(metaClassDescriptions, Config.JsonOptions);
        File.WriteAllText(path, json, Encoding.ASCII);
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
    public ArchiveBase Load(string ttarch, T3BlowfishKey game, bool sort = true, bool debugPrint = false)
    {
        return Load(ttarch, game.GetBlowfishKey(), sort, debugPrint);
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
    public ArchiveBase Load(string ttarch, string key, bool sort = true, bool debugPrint = false)
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

    /// <summary>
    /// Determines if a file is a valid Telltale MetaStream file.
    /// </summary>
    public bool IsMetaStreamFile(string fileName)
    {
        return IsMetaStreamFile(File.OpenRead(fileName));
    }

    /// <summary>
    /// Determines if a file is a valid Telltale MetaStream file.
    /// </summary>
    public bool IsMetaStreamFile(Stream stream)
    {
        return MetaStreamReader.IsValidMetaStream(stream);
    }

    /// <summary>
    /// Attempts to open and validate a Telltale file, returning configuration if successful.
    /// </summary>
    public bool TryOpenFile(string fileName, out MetaStreamConfiguration? config)
    {
        config = null;

        try
        {
            using FileStream stream = File.OpenRead(fileName);
            return TryOpenFile(stream, out config);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to open and validate a Telltale file from stream, returning configuration if successful.
    /// </summary>
    public bool TryOpenFile(Stream stream, out MetaStreamConfiguration? config)
    {
        config = null;

        try
        {
            // Quick validation using the fourcc check
            if (!MetaStreamReader.IsValidMetaStream(stream))
                return false;

            // If we need the actual configuration, parse it fully
            using var reader = new MetaStreamReader(stream);
            config = reader.Configuration;
            return true;
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// Attempts to load an object from a file, returning null if validation fails.
    /// </summary>
    public T? TryOpenObject<T>(string fileName, out MetaStreamConfiguration? config) where T : class, new()
    {
        try
        {
            using FileStream stream = File.OpenRead(fileName);
            return TryOpenObject<T>(stream, out config);
        }
        catch
        {
            config = null;
            return null;
        }
    }

    /// <summary>
    /// Attempts to load an object from a stream, returning null if validation fails.
    /// </summary>
    public T? TryOpenObject<T>(Stream stream, out MetaStreamConfiguration config) where T : class, new()
    {
        try
        {
            // First do quick validation
            if (!MetaStreamReader.IsValidMetaStream(stream))
            {
                config = null;
                return null;
            }

            // Reset and load
            stream.Position = 0;
            return LoadObject<T>(stream, out config);
        }
        catch
        {
            config = null;
            return null;
        }
    }

    /// <summary>
    /// Determines whether a file with the specified name should be considered a Meta (Telltale Tool specific) file,
    /// based on its extension.
    /// </summary>
    /// <param name="fileName">The name of the file or archive entry.</param>
    /// <returns><c>true</c> if the file is a meta file; otherwise, <c>false</c>.</returns>
    public static bool IsMetaFile(string fileName)
    {
        return Path.GetExtension(fileName) switch
        {
            ".d3dtx" or ".d3dmesh" or ".scene" or ".chore" => true,
            _ => false
            // TODO: Check for .t3fxpreloadpack
        };
    }

    /// <summary>
    /// Configuration for TelltaleToolkit initialization.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Path to the data folder containing game profiles, hash databases, etc.
        /// </summary>
        public string DataFolder { get; set; }

        /// <summary>
        /// Custom JSON serializer options for loading/saving profiles.
        /// </summary>
        public JsonSerializerOptions? JsonOptions { get; set; }
    }
}
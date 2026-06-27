using System.Text;
using System.Text.Json;
using TelltaleToolKit.Encryption;
using TelltaleToolKit.Games;
using TelltaleToolKit.Hashing;
using TelltaleToolKit.IO.Archives;
using TelltaleToolKit.IO.Archives.Formats;
using TelltaleToolKit.Logging;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit;

/// <summary>
/// The central singleton that owns global state: game profiles, the MetaClass registry,
/// the global hash database, and all active workspaces.
/// </summary>
/// <remarks>
/// <para>
/// Call <see cref="Initialize"/> once at application start before accessing
/// <see cref="Instance"/> or any other member.
/// </para>
/// <para>
/// For game-specific work (loading assets, mounting archives) prefer creating a
/// <see cref="Workspace"/> via <see cref="CreateWorkspace"/>.
/// </para>
/// <para>
/// The methods on
/// <c>Toolkit</c> itself operate without a game context and require callers to explicitly supply configurations.
/// </para>
/// </remarks>
public class Toolkit
{
    private static Toolkit? s_instance;
    private readonly Dictionary<string, GameProfile> _gameProfiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Workspace> _workspaces = new(StringComparer.OrdinalIgnoreCase);

    private Toolkit(Configuration config)
    {
        Config = config ?? throw new ArgumentNullException(nameof(config));

        ClassRegistry = new MetaClassRegistry();
        SerializerSelector = new MetaSerializerSelector();
        GlobalHashDatabase = new HashDatabase();

        LoadMetaClassDescriptions();
        LoadGameProfiles(config.DataFolder);
        LoadHashDatabase(config.DataFolder);
    }

    /// <summary>
    /// Gets whether <see cref="Initialize"/> has been called.
    /// </summary>
    public static bool IsInitialized => s_instance != null;

    /// <summary>
    /// Gets the singleton toolkit instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when accessed before <see cref="Initialize"/> has been called.
    /// </exception>
    public static Toolkit Instance
        => s_instance ?? throw new InvalidOperationException(
            "TelltaleToolkit must be initialized first. Call Toolkit.Initialize().");

    /// <summary>
    /// Gets the @params supplied at <see cref="Initialize"/> time.
    /// </summary>
    public Configuration Config { get; }

    /// <summary>
    /// Gets the logger supplied from <see cref="Config"/>.
    /// </summary>
    public IToolkitLogger Logger => Config.Logger;

    /// <summary>
    /// Gets the global metaclass registry shared across all workspaces.
    /// </summary>
    public MetaClassRegistry ClassRegistry { get; }

    /// <summary>
    /// Gets the global hash database used for symbol resolution.
    /// Workspace-local databases are checked after this one.
    /// </summary>
    public HashDatabase GlobalHashDatabase { get; }

    /// <summary>
    /// Gets the serializer selector used to pick the right serializer for a given type.
    /// </summary>
    public MetaSerializerSelector SerializerSelector { get; }

    /// <summary>
    /// Gets all registered game profiles, keyed by profile name (case-insensitive).
    /// </summary>
    public IReadOnlyDictionary<string, GameProfile> GameProfiles => _gameProfiles;

    /// <summary>
    /// Gets all workspaces currently registered with this toolkit, keyed by the workspace name
    /// (case-insensitive).
    /// </summary>
    public IReadOnlyDictionary<string, Workspace> Workspaces => _workspaces;

    // -------------------------------------------------------------------------
    // Initialization
    // -------------------------------------------------------------------------

    /// <summary>
    /// Initializes the toolkit with an optional custom <see cref="Configuration"/>.
    /// Must be called exactly once before using any other API.
    /// </summary>
    /// <param name="config">
    /// Optional @params. When <see langword="null"/>, defaults are used
    /// (<c>DataFolder</c> = <c>"ttk-data"</c>).
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when called a second time after the toolkit is already initialized.
    /// </exception>
    public static void Initialize(Configuration? config = null)
    {
        if (s_instance != null)
            throw new InvalidOperationException("TelltaleToolkit is already initialized");

        config ??= new Configuration();

        s_instance = new Toolkit(config);
    }

    // -------------------------------------------------------------------------
    // Workspace Management
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates a new <see cref="Workspace"/> for the specified game profile and registers it.
    /// </summary>
    /// <param name="workspaceName">
    /// Unique display name for the workspace (used as the key in <see cref="GetWorkspace"/>).
    /// </param>
    /// <param name="gameProfile">
    /// The name of a registered <see cref="GameProfile"/> (case-insensitive).
    /// </param>
    /// <returns>The newly created <see cref="Workspace"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no profile with the given name has been registered.
    /// </exception>
    public Workspace CreateWorkspace(string workspaceName, string gameProfile)
    {
        if (!_gameProfiles.TryGetValue(gameProfile, out GameProfile? profile))
            throw new KeyNotFoundException($"Game profile '{gameProfile}' not found");

        var workspace = new Workspace(workspaceName, this, profile);
        _workspaces[workspaceName] = workspace;
        return workspace;
    }

    /// <summary>
    /// Returns the workspace registered under <paramref name="workspaceName"/>,
    /// or <see langword="null"/> if no such workspace exists.
    /// </summary>
    /// <param name="workspaceName">The name supplied to <see cref="CreateWorkspace"/>.</param>
    public Workspace? GetWorkspace(string workspaceName)
        => _workspaces.GetValueOrDefault(workspaceName);

    // -------------------------------------------------------------------------
    // Game Profile Management
    // -------------------------------------------------------------------------

    /// <summary>
    /// Registers a game profile and loads its associated MetaClass version database.
    /// </summary>
    /// <param name="profile">The profile to register; must not be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="profile"/> is <see langword="null"/>.</exception>
    public void RegisterGameProfile(GameProfile profile)
    {
        if (profile is null)
            throw new ArgumentNullException(nameof(profile));

        _gameProfiles[profile.Name] = profile;
        LoadMetaClassDescriptionsForGame(profile);
    }

    /// <summary>
    /// Attempts to resolve the debug string for <paramref name="symbol"/> using the
    /// global hash database.
    /// </summary>
    /// <param name="symbol">The symbol to resolve must not be <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if a debug string was found and assigned to the symbol;
    /// <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="symbol"/> is <see langword="null"/>.
    /// </exception>
    public bool ResolveSymbol(Symbol symbol)
        => symbol is null ? throw new ArgumentNullException(nameof(symbol)) : GlobalHashDatabase.ResolveSymbol(symbol);

    /// <summary>
    /// Resolves multiple symbols in batch by calling <see cref="ResolveSymbol"/> on each.
    /// </summary>
    /// <param name="symbols">The symbols to resolve. Silently ignored if <see langword="null"/>.</param>
    public void ResolveSymbols(IEnumerable<Symbol> symbols)
    {
        foreach (Symbol? symbol in symbols)
            ResolveSymbol(symbol);
    }

    /// <summary>
    /// Attempts to get the debug string for a CRC64 using the global hash database.
    /// </summary>
    /// <param name="crc64">The CRC64 hash of the symbol to resolve.</param>
    /// <returns>
    /// The debug string if found; otherwise, <see langword="null"/>.
    /// </returns>
    public string? GetDebugString(ulong crc64)
        => GlobalHashDatabase.GetDebugString(crc64);

    // -------------------------------------------------------------------------
    // Serialization — Deserialize
    // -------------------------------------------------------------------------

    /// <summary>
    /// Deserializes a MetaStream object of type <typeparamref name="T"/> from a file,
    /// also returning the <see cref="MetaStreamParams"/> that was embedded in the stream.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="fileName">Path to the MetaStream file on disk.</param>
    /// <param name="workspace">Optional workspace for additional context.</param>
    /// <returns>
    /// A tuple containing the deserialized object and the @params embedded in the stream,
    /// or <c>(null, null)</c> if reading or parsing fails.
    /// </returns>
    public (T? Asset, MetaStreamParams? MetaConfig) DeserializeWithConfig<T>(string fileName, Workspace? workspace = null)
        where T : class, new()
    {
        try
        {
            using FileStream stream = File.OpenRead(fileName);
            var result = DeserializeInternal<T>(stream, workspace);

            if (result.Asset == null)
            {
                Config.Logger.LogWarning($"[Toolkit] Deserialization failed for file: {fileName}");
            }
            else
            {
                Config.Logger.LogInfo($"[Toolkit] Deserialized {typeof(T).Name} from: {fileName}");
            }

            return result;
        }
        catch (FileNotFoundException)
        {
            Config.Logger.LogWarning($"[Toolkit] File not found: {fileName}");
            return (null, null);
        }
        catch (Exception ex)
        {
            Config.Logger.LogError($"[Toolkit] Failed to deserialize {fileName}. {ex.GetType()}: {ex.Message}");
            return (null, null);
        }
    }

    /// <summary>
    /// Deserializes a MetaStream object of type <typeparamref name="T"/> from a stream,
    /// also returning the <see cref="MetaStreamParams"/> that was embedded in the stream.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="stream">
    /// A readable stream positioned at the start of a MetaStream file.
    /// Not disposed by this method.
    /// </param>
    /// <param name="workspace">Optional workspace for additional context.</param>
    /// <returns>
    /// A tuple containing the deserialized object and the @params embedded in the stream,
    /// or <c>(null, null)</c> if parsing fails.
    /// </returns>
    public (T? Asset, MetaStreamParams? MetaConfig) DeserializeWithConfig<T>(Stream stream, Workspace? workspace = null)
        where T : class, new()
    {
        var result = DeserializeInternal<T>(stream, workspace);

        if (result.Asset == null)
        {
            Config.Logger.LogWarning($"[Toolkit] Deserialization failed from stream (type: {typeof(T).Name})");
        }

        return result;
    }

    /// <summary>
    /// Deserializes a MetaStream object of type <typeparamref name="T"/> from a file,
    /// discarding the embedded @params.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="fileName">Path to the MetaStream file on disk.</param>
    /// <param name="workspace">Optional workspace for additional context.</param>
    /// <returns>
    /// The deserialized object, or <see langword="null"/> if reading or parsing fails.
    /// </returns>
    public T? Deserialize<T>(string fileName, Workspace? workspace = null) where T : class, new()
    {
        try
        {
            using FileStream stream = File.OpenRead(fileName);
            var result = DeserializeInternal<T>(stream, workspace);

            if (result.Asset == null)
            {
                Config.Logger.LogWarning($"[Toolkit] Deserialization failed for file: {fileName}");
            }

            return result.Asset;
        }
        catch (FileNotFoundException)
        {
            Config.Logger.LogWarning($"[Toolkit] File not found: {fileName}");
            return null;
        }
        catch (Exception ex)
        {
            Config.Logger.LogError($"[Toolkit] Failed to deserialize {fileName}. {ex.GetType()}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Deserializes a MetaStream object of type <typeparamref name="T"/> from a stream,
    /// discarding the embedded @params.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="stream">
    /// A readable stream positioned at the start of a MetaStream file.
    /// Not disposed by this method.
    /// </param>
    /// <param name="workspace">Optional workspace for additional context.</param>
    /// <returns>
    /// The deserialized object, or <see langword="null"/> if parsing fails.
    /// </returns>
    public T? Deserialize<T>(Stream stream, Workspace? workspace = null) where T : class, new()
    {
        var result = DeserializeInternal<T>(stream, workspace);

        if (result.Asset == null)
        {
            Config.Logger.LogWarning($"[Toolkit] Deserialization failed from stream (type: {typeof(T).Name})");
        }

        return result.Asset;
    }

    // -------------------------------------------------------------------------
    // Serialization — Serialize
    // -------------------------------------------------------------------------

    /// <summary>
    /// Serializes <paramref name="obj"/> to a MetaStream file.
    /// The file is created or overwritten.
    /// </summary>
    /// <typeparam name="T">The object type. It must have a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="fileName">Destination file path. The file is overwritten if it exists.</param>
    /// <param name="config">
    /// The <see cref="MetaStreamParams"/> controlling format version, class list, and so on.
    /// </param>
    public void Serialize<T>(T obj, string fileName, MetaStreamParams config) where T : class, new()
    {
        using FileStream stream = File.Create(fileName);
        SerializeInternal(obj, stream, config);
    }

    /// <summary>
    /// Serializes <paramref name="obj"/> to a writable stream.
    /// </summary>
    /// <typeparam name="T">The object type. It must have a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="stream">
    /// A writable stream. Not disposed by this method.
    /// </param>
    /// <param name="config">The MetaStream @params to embed in the output.</param>
    public void Serialize<T>(T obj, Stream stream, MetaStreamParams config) where T : class, new()
        => SerializeInternal(obj, stream, config);

    // -------------------------------------------------------------------------
    // Archive Loading
    // -------------------------------------------------------------------------

    /// <summary>
    /// Loads a Telltale archive file (.ttarch or .ttarch2) using a <see cref="T3BlowfishKey"/>
    /// enum value to supply the decryption key.
    /// </summary>
    /// <param name="archivePath">Path to the archive file.</param>
    /// <param name="game">Enum value identifying the game's blowfish key.</param>
    /// <returns>The loaded <see cref="Archive"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown when the file extension is not recognized.</exception>
    public Archive LoadArchive(string archivePath, T3BlowfishKey game)
        => LoadArchive(archivePath, game.GetBlowfishKey());

    /// <summary>
    /// Loads a Telltale archive file (.ttarch or .ttarch2) using a raw blowfish key string.
    /// </summary>
    /// <param name="archivePath">Path to the archive file.</param>
    /// <param name="blowfishKey">The decryption key for this game's archives.</param>
    /// <returns>The loaded <see cref="Archive"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown when the file extension is not <c>.ttarch</c> or <c>.ttarch2</c>.</exception>
    public Archive LoadArchive(string archivePath, string blowfishKey)
    {
        if (archivePath.EndsWith(".ttarch2", StringComparison.OrdinalIgnoreCase))
            return Archive.Load<TTArchive2>(archivePath, blowfishKey);

        if (archivePath.EndsWith(".ttarch", StringComparison.OrdinalIgnoreCase))
            return Archive.Load<TTArchive>(archivePath, blowfishKey);

        throw new NotSupportedException(
            $"Unsupported archive format '{Path.GetExtension(archivePath)}'. Expected .ttarch or .ttarch2.");
    }

    // -------------------------------------------------------------------------
    // MetaStream Detection
    // -------------------------------------------------------------------------

    /// <summary>
    /// Returns <see langword="true"/> if the file at <paramref name="fileName"/> is a
    /// valid Telltale MetaStream file (checked by reading the four-byte version header).
    /// </summary>
    /// <param name="fileName">Path to the file to check.</param>
    /// <returns>
    /// <see langword="true"/> if the header matches a known <see cref="MetaStreamMagic"/>;
    /// <see langword="false"/> otherwise.
    /// </returns>
    public bool IsMetaStreamFile(string fileName)
    {
        // Use a using block so the file handle is released immediately after the check.
        using FileStream stream = File.OpenRead(fileName);
        return MetaStream.IsValidMetaStream(stream);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the stream contains a valid Telltale MetaStream
    /// (checked by reading the four-byte version header).
    /// </summary>
    /// <param name="stream">
    /// A readable, seekable stream. The stream position is restored after the check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the header matches a known <see cref="MetaStreamMagic"/>;
    /// <see langword="false"/> otherwise.
    /// </returns>
    public bool IsMetaStreamFile(Stream stream)
        => MetaStream.IsValidMetaStream(stream);

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="fileName"/> has an extension
    /// associated with a Telltale MetaStream asset type
    /// (<c>.d3dtx</c>, <c>.d3dmesh</c>, <c>.scene</c>, <c>.chore</c>).
    /// </summary>
    /// <param name="fileName">The filename or archive-entry name to inspect.</param>
    /// <returns>
    /// <see langword="true"/> if the extension is a known MetaStream asset extension;
    /// <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    /// This check is extension-based only and does not read the file.
    /// Use <see cref="IsMetaStreamFile(string)"/> to validate the actual file header.
    /// <!-- TODO: add .t3fxpreloadpack and others -->
    /// </remarks>
    public static bool IsMetaFile(string fileName)
        => Path.GetExtension(fileName) switch
        {
            ".d3dtx" or ".d3dmesh" or ".scene" or ".chore" => true,
            _ => false,
        };

    /// <summary>
    /// Returns <see langword="true"/> if the stream's header matches a known
    /// <see cref="MetaStreamMagic"/> value.
    /// </summary>
    /// <param name="stream">
    /// A readable, seekable stream. The stream position is restored after the check.
    /// </param>
    public static bool IsMetaFile(Stream stream)
    {
        long savedPosition = stream.Position;
        stream.Position = 0;

        try
        {
            byte[] header = new byte[4];
            if (stream.Read(header, 0, 4) < 4)
                return false;

            var version = (MetaStreamMagic)BitConverter.ToInt32(header);
            return Enum.IsDefined(typeof(MetaStreamMagic), version);
        }
        finally
        {
            // Restore position even when Read throws.
            stream.Position = savedPosition;
        }
    }

    // -------------------------------------------------------------------------
    // Serializer Access
    // -------------------------------------------------------------------------

    /// <summary>
    /// Returns the typed serializer for <typeparamref name="T"/>.
    /// </summary>
    public MetaSerializer<T> GetSerializer<T>() => SerializerSelector.GetSerializer<T>();

    /// <summary>
    /// Returns the serializer for the given runtime <paramref name="type"/>.
    /// </summary>
    public MetaSerializer GetSerializer(Type type)
        => SerializerSelector.GetSerializer(type);

    // -------------------------------------------------------------------------
    // MetaClass Description Export
    // -------------------------------------------------------------------------

    /// <summary>
    /// Serializes the given MetaClass descriptions to a JSON file at <paramref name="path"/>.
    /// </summary>
    /// <param name="metaClassDescriptions">The descriptions to export.</param>
    /// <param name="path">Destination file path. Overwritten if it already exists.</param>
    public void ExportMetaClassDescriptions(IEnumerable<MetaClass> metaClassDescriptions, string path)
    {
        string json = JsonSerializer.Serialize(metaClassDescriptions, Config.JsonOptions);
        File.WriteAllText(path, json, Encoding.ASCII);
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private void LoadGameProfiles(string dataFolder)
    {
        string profilesPath = Path.Combine(dataFolder, "game_profiles");
        if (!Directory.Exists(profilesPath))
        {
            ToolkitLogger.ConsoleLoggerInstance.LogError($"Game profiles directory not found: {profilesPath}, from config data folder: {dataFolder}");

            return;
        }

        foreach (string file in Directory.GetFiles(profilesPath, "*.json", SearchOption.TopDirectoryOnly))
        {
            try
            {
                string json = File.ReadAllText(file);
                var gameProfile = JsonSerializer.Deserialize<GameProfile>(json, Config.JsonOptions);

                if (gameProfile != null)
                {
                    gameProfile.Id = Path.GetFileNameWithoutExtension(file);
                    RegisterGameProfile(gameProfile);
                }
            }
            catch (Exception ex)
            {
                Config.Logger?.LogWarning($"[Toolkit] Failed to load game profile '{file}': {ex.Message}");
            }
        }
    }

    private void LoadHashDatabase(string dataFolder)
    {
        string hashDbDir = Path.Combine(dataFolder, "hashdb");
        if (!Directory.Exists(hashDbDir))
            return;

        try
        {
            GlobalHashDatabase.ImportFromDirectory(hashDbDir, recursive: true);
        }
        catch (Exception ex)
        {
            Config.Logger.LogWarning($"[Toolkit] Failed to load hash databases: {ex.Message}");
        }
    }

    private void LoadMetaClassDescriptionsForGame(GameProfile profile)
    {
        string snapshotPath = Path.Combine(Config.DataFolder, "versiondb", $"{profile.Id}.vdb.json");
        if (!File.Exists(snapshotPath))
            return;

        try
        {
            string json = File.ReadAllText(snapshotPath);
            GameProfileJsonExtensions.ReadClassesJsonInto(profile, json, Config.JsonOptions);
        }
        catch (Exception ex)
        {
            Config.Logger.LogWarning(
                $"[Toolkit] Failed to load MetaClass descriptions for '{profile.Id}': {ex.Message}");
        }
    }

    private void LoadMetaClassDescriptions()
    {
        if (!Directory.Exists(Config.DataFolder))
            return;

        string globalDbPath = Path.Combine(Config.DataFolder, "versiondb", "global.vdb.json");
        if (!File.Exists(globalDbPath))
            return;

        string json = File.ReadAllText(globalDbPath);
        var metaClasses = JsonSerializer.Deserialize<List<MetaClass>>(json, Config.JsonOptions);

        if (metaClasses != null)
            ClassRegistry.Register(metaClasses);
    }

    private (T? Asset, MetaStreamParams? Config) DeserializeInternal<T>(Stream stream, Workspace? workspace = null)
        where T : class, new()
    {
        try
        {
            var result = new T();
            var metaStream = MetaStream.OpenRead(stream, workspace);

            metaStream.Serialize(ref result);

            MetaStreamParams config = metaStream.Params;

            if (metaStream.IsEndOfStream())
            {
                Config.Logger.LogInfo(
                    $"[Toolkit] Successfully read {typeof(T).Name}, stream version: {config.StreamVersion}.");
            }
            else
            {
                Config.Logger.LogWarning($"[Toolkit] Unexpected end of stream while reading {typeof(T).Name}.");
            }

            if (Config.AutoResolveSymbols)
            {
                ResolveSymbols(config.SerializedSymbols);
            }

            metaStream.Close();
            return (result, config);
        }
        catch (Exception ex)
        {
            // Internal method only logs debug details, doesn't warn/error
            Config.Logger.LogError($"[Toolkit] Deserialization internal error: {ex.Message} {ex.StackTrace}");
            return (null, null);
        }
    }

    private void SerializeInternal<T>(T obj, Stream stream, MetaStreamParams config)
        where T : class, new()
    {
        var metaStream = MetaStream.OpenWrite(stream, config);
        T refObj = obj;
        metaStream.Serialize(ref refObj);
        metaStream.Close();
    }

    /// <summary>
    /// Params supplied to <see cref="Initialize"/>.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Path to the data folder that contains <c>game_profiles/</c>, <c>hashdb/</c>,
        /// and <c>versiondb/</c> subdirectories
        /// Defaults to <c>"ttk-data"</c> (relative to the working directory).
        /// </summary>
        public string DataFolder { get; set; } = "ttk-data";

        /// <summary>
        /// Gets or sets a value indicating whether symbols found in the
        /// <see cref="MetaStreamParams"/> table are automatically
        /// resolved against the global hash database after every successful
        /// <see cref="Deserialize{T}(string, Workspace?)"/> call.
        /// <para>
        /// Individual call sites can override this with the <c>autoResolveSymbols</c>
        /// parameter: <see langword="true"/> forces resolution, <see langword="false"/>
        /// suppresses it, and <see langword="null"/> (the default) defers to this flag.
        /// </para>
        /// Defaults to <see langword="true"/>.
        /// </summary>
        public bool AutoResolveSymbols { get; set; } = true;

        /// <summary>
        /// JSON serializer options used when reading game profiles and MetaClass version databases.
        /// Override to customize converters, indentation, or encoding.
        /// </summary>
        public JsonSerializerOptions JsonOptions { get; set; } = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() },
            WriteIndented = true
        };

        /// <summary>
        /// Gets or sets the logger used for internal warnings and diagnostics.
        /// When <see langword="null"/>, messages are silently discarded.
        /// Assign an instance such as <see cref="ToolkitLogger.ConsoleLoggerInstance"/> to see output.
        /// </summary>
        public IToolkitLogger Logger { get; set; } = ToolkitLogger.Null;
    }
}

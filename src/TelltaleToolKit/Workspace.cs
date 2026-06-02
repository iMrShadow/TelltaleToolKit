using System.Text;
using Lua;
using TelltaleToolKit.Encryption;
using TelltaleToolKit.Games;
using TelltaleToolKit.Hashing;
using TelltaleToolKit.IO.Archives;
using TelltaleToolKit.IO.Resources;
using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit;

/// <summary>
/// A self-contained environment for working with the assets of a specific Telltale game.
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="Workspace"/> ties together a <see cref="GameProfile"/> (blowfish key,
/// MetaStream version, class registry) with an ordered collection of
/// <see cref="ResourceContext"/> objects that supply the actual file data.
/// </para>
/// <para>
/// The typical setup for a game that uses resource-description Lua scripts (Version 2):
/// <code>
/// Toolkit.Initialize();
/// Workspace ws = Toolkit.Instance.CreateWorkspace("TWD", "The Walking Dead: Definitive Series");
/// ws.LoadResourceDescription(@"C:\GameData\WalkingDead.resdesc");
/// Scene = ws.LoadAsset&lt;Scene&gt;("adv_backwoodsStream.scene");
/// </code>
/// For older games that expose a flat folder (Version 1):
/// <code>
/// ws.MountGameFolder(@"C:\GameData", priority: 0);
/// ws.MountGameFolder(@"C:\GameData\Patch", priority: 10);
/// </code>
/// Or mount a single archive directly:
/// <code>
/// ws.LoadArchive(@"C:\GameData\WDC_pc_WalkingDead201_data.ttarch2", "My Archive");
/// </code>
/// </para>
/// <para>
/// During asset lookups, contexts are searched from the highest <see cref="ResourceContext.Priority"/>
/// value to the lowest, so a patch or mod at priority 10 overrides base-game data at priority 0.
/// </para>
/// </remarks>
public class Workspace
{
    private const string LEnHeader = "\eLEn";
    private const string LuaHeader = "\eLua";
    private const string LEoHeader = "\eLEo";

    // Per-instance list so descriptions from different workspaces don't bleed into each other.
    private readonly List<ResourceContext> _contexts = [];
    private readonly List<LuaTable> _extractedResourceDescriptions = [];
    private readonly Toolkit _toolkit;
    private LuaState? _resdecLuaState;

    internal Workspace(string name, Toolkit toolkit, GameProfile profile)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _toolkit = toolkit ?? throw new ArgumentNullException(nameof(toolkit));
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        LocalHashDatabase = new HashDatabase();

        bool isModifiedBlowfish = Profile.TtarchVersion >= TTArchiveVersion.Seven;
        Blowfish = new Blowfish(profile.BlowfishKey, isModifiedBlowfish);

        DefaultMetaStreamConfig = new MetaStreamParams
        {
            StreamVersion = profile.StreamVersion,
            Workspace = this,
            CanModifySerializedClassesList = true
        };
    }

    /// <summary>Gets the game profile that describes encryption, versioning, and class registry for this workspace.</summary>
    public GameProfile Profile { get; }

    /// <summary>Gets or sets the display name of this workspace.</summary>
    public string Name { get; set; }

    /// <summary>Gets the human-readable name of the associated game (from <see cref="Profile"/>).</summary>
    public string GameName => Profile.Name;

    /// <summary>
    /// Gets the default <see cref="MetaStreamParams"/> for this workspace,
    /// pre-populated from the game profile.
    /// Passed to serialization helpers unless overridden by the caller.
    /// </summary>
    public MetaStreamParams DefaultMetaStreamConfig { get; }

    /// <summary>Gets the blowfish key used to decrypt archives and resource descriptions for this game.</summary>
    public string BlowfishKey => Profile.BlowfishKey;

    /// <summary>Gets the blowfish instance used to decrypt archives and resource descriptions for this game.</summary>
    public Blowfish Blowfish { get; }

    /// <summary>
    /// Gets the workspace-local hash database used for symbol resolution.
    /// Entries here complement (and are checked after) the global database in <see cref="Toolkit"/>.
    /// </summary>
    public HashDatabase LocalHashDatabase { get; }

    /// <summary>
    /// Gets or sets a value indicating whether symbols are automatically resolved after
    /// every successful <see cref="LoadAsset{T}(string)"/> call on this workspace.
    /// <para>
    /// Resolution uses <see cref="ResolveSymbols"/>, which checks the global database,
    /// the workspace-local database, and the loaded archives — in that order.
    /// </para>
    /// <para>
    /// Individual call sites can override this with an <c>autoResolveSymbols</c>
    /// parameter: <see langword="true"/> forces resolution, <see langword="false"/>
    /// suppresses it, and <see langword="null"/> (the default) defers to this property.
    /// </para>
    /// Defaults to <see langword="true"/>.
    /// </summary>
    public bool AutoResolveSymbols { get; set; } = true;

    /// <summary>
    /// Gets a read-only snapshot of all resource contexts currently registered with this workspace,
    /// ordered from lowest to highest <see cref="ResourceContext.Priority"/>.
    /// </summary>
    public IReadOnlyList<ResourceContext> Contexts => _contexts;

    /// <summary>
    /// Gets the <see cref="LuaState"/> used to evaluate resource-description (.resdesc) Lua scripts.
    /// Created lazily on first access.
    /// </summary>
    public LuaState ResdecLuaState
    {
        get
        {
            if (_resdecLuaState != null) return _resdecLuaState;

            _resdecLuaState = LuaState.Create();
            SetupLuaState(_resdecLuaState, _extractedResourceDescriptions);
            return _resdecLuaState;
        }
    }

    /// <summary>
    /// Raised after an archive has been loaded and is ready for use.
    /// Subscribe to track which archives are open, e.g., for progress reporting.
    /// </summary>
    public event Action<Archive>? ArchiveLoaded;

    /// <summary>
    /// Raised just before an archive is unloaded and its resources released.
    /// </summary>
    public event Action<Archive>? ArchiveUnloaded;

    #region Version 1: Game Folder Mounting

    /// <summary>
    /// Mounts an entire game folder (and its subfolders) as a resource context.
    /// </summary>
    /// <remarks>
    /// Use this for Version 1 games that store data in a flat directory layout.
    /// Typical convention: priority 0 for the main game folder, 10 for a patch folder.
    /// A <see cref="FolderProvider"/> is created internally so loose files, .ttarch
    /// archives, and .ttarch2 archives inside the folder are all accessible.
    /// </remarks>
    /// <param name="path">Absolute path to the root game folder.</param>
    /// <param name="priority">
    /// Priority relative to other contexts.  Higher values override lower values during
    /// asset lookup.
    /// </param>
    /// <returns>The newly created and registered <see cref="ResourceContext"/>.</returns>
    public ResourceContext MountGameFolder(string path, int priority)
    {
        var context = new ResourceContext($"Folder:{Path.GetFileName(path)}", priority);
        context.AddProvider(new FolderProvider(path, this));
        AddResourceContextSorted(context);
        return context;
    }

    #endregion

    #region Version 2: Resource Description Loading

    /// <summary>
    /// Parses a resource-description Lua script (.resdesc) and mounts all archives
    /// it references as a new <see cref="ResourceContext"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The name and priority of the resulting context are taken from the script's
    /// <c>name</c> and <c>priority</c> fields.  Archives listed in
    /// <c>gameDataArchives</c> are wrapped in <see cref="ArchiveProvider"/> instances.
    /// Loose files in the same directory as the script (excluding .ttarch*, .ttarch2*,
    /// and .lua files) are also added as <see cref="LooseFileProvider"/> instances.
    /// </para>
    /// <para>
    /// Compiled (.eLEn / .eLEo) resource descriptions are not yet supported and will
    /// cause an <see cref="ArgumentException"/> to be thrown.
    /// </para>
    /// </remarks>
    /// <param name="descPath">Absolute or relative path to the .resdesc (Lua) file.</param>
    /// <returns>
    /// The newly created and registered <see cref="ResourceContext"/>, or
    /// <see langword="null"/> if the script does not contain a valid
    /// <c>gameDataArchives</c> table.
    /// </returns>
    public async Task<ResourceContext?> LoadResourceDescriptionAsync(string descPath)
    {
        LuaTable resdesc = await ParseResourceDescriptionAsync(descPath);

        string name = resdesc["name"].Read<string>();
        int priority = resdesc["priority"].Read<int>();

        var context = new ResourceContext(name, priority);

        LuaValue archives = resdesc["gameDataArchives"];

        if (archives.TryRead(out LuaTable table))
        {
            AddGameDataArchives(descPath, table, context);
        }
        else
        {
            _toolkit.Config.Logger.LogWarning(
                $"[Workspace:{Name}] Resource description '{descPath}' has no 'gameDataArchives' table - context not created.");
            return null;
        }

        //idea for this bit, make it so that they're only added to one context in the Workspace,
        //  not every ResourceContext this generates - Gamma

        // Add loose files from the base folder
        string? baseFolder = Path.GetDirectoryName(descPath);
        if (baseFolder != null)
        {
            foreach (string file in Directory.GetFiles(baseFolder))
            {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                if (ext is not ".ttarch" and not ".ttarch2" and not ".lua")
                    context.AddProvider(new LooseFileProvider(file));
            }
        }

        AddResourceContextSorted(context);
        return context;
    }

    #endregion

    private void AddResourceContextSorted(ResourceContext ctx)
    {
        _contexts.Add(ctx);
        _contexts.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }

    private void SetupLuaState(LuaState state, List<LuaTable> sink)
    {
        state.Environment["RegisterSetDescription"] = new LuaFunction((context, _) =>
        {
            sink.Add(context.GetArgument<LuaTable>(0));
            context.Return();
            return new ValueTask<int>(0);
        });

        state.Environment["_currentDirectory"] = "";
    }

    private void AddGameDataArchives(string descPath, LuaTable archives, ResourceContext context)
    {
        foreach (KeyValuePair<LuaValue, LuaValue> kvPair in archives.ToArray())
        {
            string archivePath = kvPair.Value.Read<string>();

            string fullPath = Path.IsPathRooted(archivePath)
                ? archivePath
                : Path.Combine(Path.GetDirectoryName(descPath)!, archivePath);

            context.AddProvider(new ArchiveProvider(fullPath, this));
        }
    }

    //todo (gamma): try executing compiled resdescs (through decomp)
    // and return null if they're not actually resdescs
    private async Task<LuaTable> ParseResourceDescriptionAsync(string descPath)
    {
        await using FileStream luaFile = File.OpenRead(descPath);

        byte[] headerBytes = new byte[4];
        int read = luaFile.Read(headerBytes, 0, 4);
        if (read < 4)
            throw new IOException($"Could not read header from Lua file: {descPath}");

        string header = Encoding.ASCII.GetString(headerBytes);

        if (header is not LEnHeader and not LEoHeader)
            luaFile.Position = 0;

        byte[] fileBytes = new byte[luaFile.Length - luaFile.Position];
        read = luaFile.Read(fileBytes, 0, fileBytes.Length);
        if (read < fileBytes.Length)
            throw new IOException($"Could not read body from Lua file: {descPath}");

        var blowfishInstance = new Blowfish(Profile.BlowfishKey, 7);
        blowfishInstance.Decipher(fileBytes, fileBytes.Length);

        if (header is LEnHeader)
        {
            byte[] newLua = new byte[fileBytes.Length + 4];
            Array.Copy(Encoding.ASCII.GetBytes(LuaHeader), newLua, 4);
            Array.Copy(fileBytes, 0, newLua, 4, fileBytes.Length);
            //is this the best way? just seeing if it works for right now
            //it does not, working on it
            // string tempFilePath = Path.GetTempFileName();
            // File.WriteAllBytes(tempFilePath, newLua);

            // Compiled Lua bytecode in a resdesc is not yet supported.
            throw new NotSupportedException(
                $"Compiled resource description files are not yet supported: {descPath}");
        }

        string lua = Encoding.ASCII.GetString(fileBytes);
        await ResdecLuaState.DoStringAsync(lua);

        return _extractedResourceDescriptions.Last();
    }

    /// <summary>
    /// Recursively walks a provider list and returns the first leaf provider that
    /// contains <paramref name="crc64"/>, or <see langword="null"/> if none does.
    /// </summary>
    private static IFileProvider? FindProviderRecursive(IEnumerable<IFileProvider> providers, ulong crc64)
    {
        foreach (IFileProvider provider in providers)
        {
            if (provider is FolderProvider folder)
            {
                IFileProvider? found = FindProviderRecursive(folder.GetAllProviders(), crc64);
                if (found != null) return found;
            }
            else if (provider.ContainsFile(crc64))
            {
                return provider;
            }
        }

        return null;
    }

    #region Archive Management

    /// <summary>
    /// Loads a Telltale archive from disk, fires the <see cref="ArchiveLoaded"/> event,
    /// and returns the raw <see cref="Archive"/>.
    /// </summary>
    /// <remarks>
    /// Prefer the overload that returns a <see cref="ResourceContext"/> when you want the
    /// archive registered with the workspace's priority system.  Call this overload when you
    /// need to pass the raw archive to a lower-level API.
    /// </remarks>
    /// <param name="archivePath">Absolute or relative path to the .ttarch / .ttarch2 file.</param>
    /// <returns>The loaded <see cref="Archive"/>.</returns>
    public Archive LoadArchive(string archivePath)
    {
        Archive archive = _toolkit.LoadArchive(archivePath, BlowfishKey);
        ArchiveLoaded?.Invoke(archive);
        return archive;
    }

    /// <summary>
    /// Loads a single archive and wraps it in a new <see cref="ResourceContext"/>.
    /// </summary>
    /// <remarks>
    /// Useful when you want to mount one specific archive without a resource-description
    /// file, e.g., for quick inspection or patching workflows.
    /// </remarks>
    /// <param name="archivePath">Absolute or relative path to the .ttarch / .ttarch2 file.</param>
    /// <param name="contextName">Human-readable name for the resulting context.</param>
    /// <param name="priority">Priority relative to other contexts (default: 1000).</param>
    /// <returns>The newly created and registered <see cref="ResourceContext"/>.</returns>
    public ResourceContext LoadArchive(string archivePath, string contextName, int priority = 1000)
    {
        ResourceContext context = CreateResourceContext(contextName, priority);
        context.AddProvider(new ArchiveProvider(archivePath, this));
        return context;
    }

    #endregion

    #region MetaClass Resolution

    /// <summary>
    /// Looks up the <see cref="MetaClass"/> description for <paramref name="type"/>
    /// within this workspace's game profile.
    /// </summary>
    /// <param name="type">The CLR type whose metaclass description is needed.</param>
    /// <returns>
    /// The matching <see cref="MetaClass"/>, or <see langword="null"/> if the type is
    /// not registered for this game.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <see langword="null"/>.</exception>
    public MetaClass? GetMetaClassDescription(Type? type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        KeyValuePair<MetaClassType, uint> match =
            Profile.Classes.FirstOrDefault(tc => tc.Key.LinkingType == type);

        if (match.Key == null || match.Value == 0)
            return null;

        return Toolkit.Instance.ClassRegistry.GetClass(match.Key.Symbol, match.Value);
    }

    /// <summary>
    /// Looks up the <see cref="MetaClass"/> description for <paramref name="symbol"/>
    /// within this workspace's game profile.
    /// </summary>
    /// <param name="symbol">The symbol whose metaclass description is needed.</param>
    /// <returns>
    /// The matching <see cref="MetaClass"/>, or <see langword="null"/> if the symbol is
    /// not registered for this game.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="symbol"/> is <see langword="null"/>.</exception>
    public MetaClass? GetMetaClassDescription(Symbol? symbol)
    {
        if (symbol is null)
            throw new ArgumentNullException(nameof(symbol));

        KeyValuePair<MetaClassType, uint> match = Profile.Classes
            .FirstOrDefault(tc => tc.Key.Symbol.Crc64 == symbol.Crc64);

        if (match.Key == null || match.Value == 0)
            return null;

        return Toolkit.Instance.ClassRegistry.GetClass(match.Key.Symbol, match.Value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="desc"/> is registered in this
    /// workspace's game profile.
    /// </summary>
    public bool IsMetaClassDescriptionRegistered(MetaClass? desc)
        => desc is not null && Profile.Classes.ContainsKey(desc.ClassType);

    #endregion

    #region Resource Context Management

    /// <summary>
    /// Creates and registers a new, empty <see cref="ResourceContext"/> with this workspace.
    /// Add providers to it with <see cref="ResourceContext.AddProvider"/> afterwards.
    /// </summary>
    /// <param name="name">Human-readable name for lookup and diagnostics.</param>
    /// <param name="priority">
    /// Priority relative to other contexts.
    /// Higher values are searched first during asset loading.
    /// </param>
    /// <returns>The newly created <see cref="ResourceContext"/>.</returns>
    public ResourceContext CreateResourceContext(string name, int priority)
    {
        var context = new ResourceContext(name, priority);
        AddResourceContextSorted(context);
        return context;
    }

    /// <summary>
    /// Returns the first context whose <see cref="ResourceContext.Name"/> matches
    /// <paramref name="name"/> (ordinal, case-insensitive), or <see langword="null"/> if none found.
    /// </summary>
    public ResourceContext? GetResourceContext(string name)
        => _contexts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Unloads and removes the context with the given name.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the context was found and removed;
    /// <see langword="false"/> if no context with that name was registered.
    /// </returns>
    public bool RemoveResourceContext(string name)
    {
        ResourceContext? context = GetResourceContext(name);
        if (context == null) return false;

        context.Unload();
        return _contexts.Remove(context);
    }

    /// <summary>
    /// Enables the context with the given name so it participates in asset lookups.
    /// </summary>
    /// <param name="name">Name of the context to enable.</param>
    public void EnableContext(string name)
        => GetResourceContext(name)?.Enable();

    /// <summary>
    /// Disables the context with the given name so it is skipped during asset lookups.
    /// </summary>
    /// <param name="name">Name of the context to disable.</param>
    public void DisableContext(string name)
        => GetResourceContext(name)?.Disable();

    /// <summary>
    /// Unloads all registered resource contexts and clears the context list.
    /// </summary>
    public void ClearAllContexts()
    {
        foreach (ResourceContext? context in _contexts)
            context.Unload();
        _contexts.Clear();
    }

    #endregion

    #region File Operations

    /// <summary>
    /// Loads and deserializes an asset of type <typeparamref name="T"/> by filename.
    /// Contexts are searched from the highest priority to the lowest; the first match wins.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="name">The bare filename of the asset (for example, <c>adv_backwoodsStream.scene</c>).</param>
    /// <returns>
    /// The deserialized asset, or <see langword="null"/> if the asset was not found in any context.
    /// </returns>
    public T? LoadAsset<T>(string name) where T : class, new()
        => LoadAsset<T>(Crc64.Compute(name));

    /// <summary>
    /// Loads and deserializes an asset of type <typeparamref name="T"/> by CRC-64.
    /// Contexts are searched from the highest priority to lowest; the first match wins.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="crc64">Pre-computed CRC-64 of the asset filename.</param>
    /// <returns>
    /// The deserialized asset, or <see langword="null"/> if the asset was not found in any context.
    /// </returns>
    public T? LoadAsset<T>(ulong crc64) where T : class, new()
        => LoadAssetWithConfig<T>(crc64).Asset;

    /// <summary>
    /// Loads and deserializes an asset of type <typeparamref name="T"/> by filename, also
    /// returning the <see cref="MetaStreamParams"/> embedded in the stream.
    /// </summary>
    /// <typeparam name="T">The target typ. It must have a parameterless constructor.</typeparam>
    /// <param name="name">The bare filename of the asset.</param>
    /// <returns>
    /// A tuple containing the deserialized asset and its @params,
    /// or <c>(null, null)</c> if not found.
    /// </returns>
    public (T? Asset, MetaStreamParams? MetaConfig) LoadAssetWithConfig<T>(string name) where T : class, new()
        => LoadAssetWithConfig<T>(Crc64.Compute(name));

    /// <summary>
    /// Loads and deserializes an asset of type <typeparamref name="T"/> by CRC-64, also
    /// returning the <see cref="MetaStreamParams"/> embedded in the stream.
    /// </summary>
    /// <typeparam name="T">The target type. It must have a parameterless constructor.</typeparam>
    /// <param name="crc64">Pre-computed CRC-64 of the asset filename.</param>
    /// <returns>
    /// A tuple containing the deserialized asset and its @params,
    /// or <c>(null, null)</c> if not found.
    /// </returns>
    public (T? Asset, MetaStreamParams? MetaConfig) LoadAssetWithConfig<T>(ulong crc64) where T : class, new()
        => LoadAssetInternal<T>(crc64);

    private (T? Value, MetaStreamParams? MetaConfig) LoadAssetInternal<T>(ulong crc64) where T : class, new()
    {
        for (int i = _contexts.Count - 1; i >= 0; i--)
        {
            if (!_contexts[i].IsEnabled) continue;

            Stream? stream = _contexts[i].ExtractFile(crc64);
            if (stream == null) continue;

            string? assetName = GetDebugString(crc64);

            _toolkit.Config.Logger.LogInfo(assetName != null
                ? $"[Workspace:{Name}] Found asset '{assetName}' in context: {_contexts[i].Name}"
                : $"[Workspace:{Name}] Found asset 0x{crc64:X16} in context: {_contexts[i].Name}");

            var asset = _toolkit.DeserializeWithConfig<T>(stream, this);

            if (asset.Asset == null)
            {
                _toolkit.Config.Logger.LogWarning(
                    $"[Workspace:{Name}] Asset {(assetName ?? $"0x{crc64:X16}")} found but failed to load in context: {_contexts[i].Name}");
                return (null, null);
            }

            if (AutoResolveSymbols && asset.MetaConfig != null)
                ResolveSymbols(asset.MetaConfig.SerializedSymbols);

            return asset;
        }

        _toolkit.Config.Logger.LogInfo($"[Workspace:{Name}] Asset 0x{crc64:X16} not found in any enabled context");
        return (null, null);
    }

    /// <summary>
    /// Serializes <paramref name="obj"/> to a file on disk using this workspace's
    /// <see cref="DefaultMetaStreamConfig"/>.
    /// </summary>
    /// <remarks>
    /// This is the counterpart to <see cref="LoadAsset{T}(string)"/>.
    /// The workspace's game profile determines the MetaStream version, symbol hashing,
    /// and class-list behavior — so the output is always valid for the target game
    /// without any extra configuration.
    /// </remarks>
    /// <typeparam name="T">The asset type; must have a parameterless constructor.</typeparam>
    /// <param name="obj">The asset to serialize.</param>
    /// <param name="fileName">
    /// Destination file path on disk.
    /// The file is created or overwritten.
    /// The filename (without the path) should match the original asset name
    /// so the game can find it by CRC-64 hash.
    /// </param>
    public void ExportAsset<T>(T obj, string fileName) where T : class, new()
        => _toolkit.Serialize(obj, fileName, DefaultMetaStreamConfig);

    /// <summary>
    /// Serializes <paramref name="obj"/> to a file on disk using a custom
    /// <see cref="MetaStreamParams"/>.
    /// </summary>
    /// <remarks>
    /// Use this overload when you need to override the MetaStream version or class-list
    /// settings that <see cref="DefaultMetaStreamConfig"/> supplies.
    /// For the common case, prefer <see cref="ExportAsset{T}(T, string)"/>.
    /// </remarks>
    /// <typeparam name="T">The asset type; must have a parameterless constructor.</typeparam>
    /// <param name="obj">The asset to serialize.</param>
    /// <param name="fileName">
    /// Destination file path on disk.
    /// The file is created or overwritten.
    /// </param>
    /// <param name="config">The MetaStream @params to embed in the output.</param>
    public void ExportAsset<T>(T obj, string fileName, MetaStreamParams config) where T : class, new()
        => _toolkit.Serialize(obj, fileName, config);

    /// <summary>
    /// Serializes <paramref name="obj"/> to a writable stream using this workspace's
    /// <see cref="DefaultMetaStreamConfig"/>.
    /// </summary>
    /// <typeparam name="T">The asset type; must have a parameterless constructor.</typeparam>
    /// <param name="obj">The asset to serialize.</param>
    /// <param name="stream">
    /// A writable stream. Not disposed by this method.
    /// </param>
    public void ExportAsset<T>(T obj, Stream stream) where T : class, new()
        => _toolkit.Serialize(obj, stream, DefaultMetaStreamConfig);

    /// <summary>
    /// Opens a raw stream for a file by name, searching enabled contexts from the highest
    /// priority to the lowest.
    /// </summary>
    /// <param name="name">The bare filename of the asset.</param>
    /// <returns>
    /// A readable stream positioned at the start of the file data, or
    /// <see langword="null"/> if the file was not found in any enabled context.
    /// The caller is responsible for disposing of the stream.
    /// </returns>
    public Stream? ExtractFile(string name)
        => ExtractFile(Crc64.Compute(name));

    /// <summary>
    /// Opens a raw stream for a file by CRC-64, searching enabled contexts from the highest
    /// priority to the lowest.
    /// </summary>
    /// <param name="crc64">Pre-computed CRC-64 of the filename.</param>
    /// <returns>
    /// A readable stream, or <see langword="null"/> if not found.
    /// The caller is responsible for disposing of the stream.
    /// </returns>
    public Stream? ExtractFile(ulong crc64)
    {
        // Search from highest priority to lowest (so highest overrides)
        for (int i = _contexts.Count - 1; i >= 0; i--)
        {
            if (!_contexts[i].IsEnabled) continue;

            Stream? stream = _contexts[i].ExtractFile(crc64);
            if (stream != null) return stream;
        }

        return null;
    }

    /// <summary>
    /// Returns <see langword="true"/> if any enabled context contains a file with the given CRC-64.
    /// </summary>
    public bool ContainsFile(ulong crc64)
        => _contexts.Any(c => c.IsEnabled && c.ContainsFile(crc64));

    /// <summary>
    /// Returns <see langword="true"/> if any enabled context contains a file with the given name.
    /// </summary>
    public bool ContainsFile(string name)
        => ContainsFile(Crc64.Compute(name));

    /// <summary>
    /// Finds and returns the <see cref="ResourceEntry"/> for the given CRC-64,
    /// searching enabled contexts from the highest priority to lowest.
    /// </summary>
    /// <returns>The entry, or <see langword="null"/> if not found.</returns>
    public ResourceEntry? FindFileEntry(ulong crc64)
    {
        for (int i = _contexts.Count - 1; i >= 0; i--)
        {
            if (!_contexts[i].IsEnabled) continue;

            ResourceEntry? entry = _contexts[i].GetFileEntry(crc64);
            if (entry != null) return entry;
        }

        return null;
    }

    /// <summary>
    /// Finds the first <see cref="IFileProvider"/> in any enabled context that contains
    /// the file with the given CRC-64, searching from the highest priority to lowest.
    /// </summary>
    /// <remarks>
    /// Recursively inspects <see cref="FolderProvider"/> instances so that files nested
    /// inside folder hierarchies are also reachable.
    /// </remarks>
    /// <param name="crc64">Pre-computed CRC-64 of the filename.</param>
    /// <returns>The provider, or <see langword="null"/> if no provider contains the file.</returns>
    public IFileProvider? GetFileProviderForResource(ulong crc64)
    {
        for (int i = _contexts.Count - 1; i >= 0; i--)
        {
            if (!_contexts[i].IsEnabled) continue;

            IFileProvider? found = FindProviderRecursive(_contexts[i].Providers, crc64);
            if (found != null) return found;
        }

        return null;
    }

    /// <summary>
    /// Lists all file entries across all enabled contexts.
    /// </summary>
    /// <remarks>
    /// May contain duplicates when the same file is present in multiple contexts.
    /// Use <see cref="FindFileEntry"/> if you want the highest-priority entry for a
    /// specific file.
    /// </remarks>
    /// <returns>
    /// A flat sequence of <see cref="ResourceEntry"/> objects from all enabled contexts.
    /// </returns>
    public IEnumerable<ResourceEntry> GetAllEntries()
        => _contexts.Where(c => c.IsEnabled).SelectMany(c => c.GetAllEntries());

    #endregion

    #region Symbol Resolution

    /// <summary>
    /// Attempts to get the debug string for a CRC64 using the workspace's symbol database.
    /// </summary>
    /// <param name="crc64">The CRC64 hash of the symbol to resolve.</param>
    /// <returns>
    /// The debug string if found; otherwise, <see langword="null"/>.
    /// </returns>
    public string? GetDebugString(ulong crc64)
    {
        string? debugString = _toolkit.GetDebugString(crc64);
        if (debugString != null)
            return debugString;

        debugString = LocalHashDatabase.GetDebugString(crc64);
        if (debugString != null)
            return debugString;

        var fileEntry = FindFileEntry(crc64);
        if (fileEntry?.Name != null)
        {
            _toolkit.Config.Logger.LogInfo(
                $"[Workspace:{Name}] Resolved 0x{crc64:X16} as '{fileEntry.Name}' from archive");
            return fileEntry.Name;
        }

        _toolkit.Config.Logger.LogInfo($"[Workspace:{Name}] Failed to resolve 0x{crc64:X16}");
        return null;
    }

    /// <summary>
    /// Attempts to resolve the debug string for <paramref name="symbol"/> by checking,
    /// in order: (1) the global hash database, (2) the workspace-local hash database,
    /// (3) the file-entry table of all loaded archives.
    /// </summary>
    /// <param name="symbol">The symbol to resolve.</param>
    /// <returns><see langword="true"/> if a debug string was found and assigned.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="symbol"/> is <see langword="null"/>.</exception>
    public bool ResolveSymbol(Symbol? symbol)
    {
        if (symbol is null)
            throw new ArgumentNullException(nameof(symbol));

        if (symbol.DebugString is not null) return true;
        if (_toolkit.ResolveSymbol(symbol)) return true;
        if (LocalHashDatabase.ResolveSymbol(symbol)) return true;

        ResourceEntry? fileEntry = FindFileEntry(symbol.Crc64);
        if (fileEntry?.Name != null)
        {
            symbol.Resolve(fileEntry.Name);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resolves multiple symbols in a batch.
    /// Each symbol is passed to <see cref="ResolveSymbol"/>.
    /// </summary>
    /// <param name="symbols">The symbols to resolve.</param>
    public void ResolveSymbols(IEnumerable<Symbol>? symbols)
    {
        if (symbols == null)
            return;

        foreach (Symbol? symbol in symbols)
            ResolveSymbol(symbol);
    }

    #endregion
}

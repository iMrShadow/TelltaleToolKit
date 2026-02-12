using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Resource;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit;

public class Workspace
{
    private readonly SortedDictionary<int, ResourceContext> _contextsByPriority = new();
    private readonly Toolkit _toolkit;

    internal Workspace(string name, Toolkit toolkit, GameProfile profile)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _toolkit = toolkit ?? throw new ArgumentNullException(nameof(toolkit));
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        LocalHashDatabase = new HashDatabase.HashDatabase();

        DefaultMetaStreamConfig = new MetaStreamConfiguration
        {
            AreSymbolsHashed = profile.AreSymbolsHashed,
            Version = profile.MetaStreamVersion,
        };
    }

    /// <summary>
    /// Gets the game descriptor for this workspace.
    /// </summary>
    public GameProfile Profile { get; }

    /// <summary>
    /// Gets or sets the name of the workspace.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the name of the game.
    /// </summary>
    public string GameName => Profile.Name;

    /// <summary>
    /// Gets the default <see cref="MetaStreamConfiguration"/> for the currently active game.
    /// </summary>
    public MetaStreamConfiguration DefaultMetaStreamConfig { get; }

    /// <summary>
    /// Gets the blowfish key for this game.
    /// </summary>
    public string BlowfishKey => Profile.BlowfishKey;

    /// <summary>
    /// Gets the hash database for this game.
    /// </summary>
    public HashDatabase.HashDatabase LocalHashDatabase { get; }

    /// <summary>
    /// Event raised when an archive is loaded.
    /// </summary>
    public event Action<ArchiveBase> ArchiveLoaded;

    /// <summary>
    /// Event raised when an archive is unloaded.
    /// </summary>
    public event Action<ArchiveBase> ArchiveUnloaded;

    #region MetaClass Resolution

    public MetaClass? GetMetaClassDescription(Type? type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        KeyValuePair<MetaClassType, uint> match =
            Profile.Classes.FirstOrDefault(tc => tc.Key.LinkingType == type);

        if (match.Key == null || match.Value == 0)
            return null;

        return Toolkit.Instance.ClassRegistry.GetClass(match.Key.Symbol, match.Value);
    }

    public MetaClass? GetMetaClassDescription(Symbol? symbol)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        KeyValuePair<MetaClassType, uint> match = Profile.Classes
            .FirstOrDefault(tc => tc.Key.Symbol.Crc64 == symbol.Crc64);

        if (match.Key == null || match.Value == 0)
            return null;

        return Toolkit.Instance.ClassRegistry.GetClass(match.Key.Symbol, match.Value);
    }

    public bool IsMetaClassDescriptionRegistered(MetaClass? desc)
        => desc is not null && Profile.Classes.ContainsKey(desc.ClassType);

    #endregion

    #region Resource Context Management

    /// <summary>
    /// Creates a new resource context with explicit priority.
    /// If a context with this priority already exists, it is replaced.
    /// </summary>
    public ResourceContext CreateResourceContext(string name, int priority)
    {
        var context = new ResourceContext(name, priority, this);
        _contextsByPriority[priority] = context; // Dictionary, not list - priority is unique
        return context;
    }

    /// <summary>
    /// Gets a resource context by name.
    /// </summary>
    public ResourceContext? GetResourceContext(string name)
        => _contextsByPriority.Values.FirstOrDefault(context => context.Name == name);

    /// <summary>
    /// Gets a resource context by priority.
    /// </summary>
    public ResourceContext? GetResourceContext(int priority)
        => _contextsByPriority.TryGetValue(priority, out ResourceContext? context) ? context : null;


    /// <summary>
    /// Removes a resource context by name.
    /// </summary>
    public bool RemoveResourceContext(string name)
    {
        ResourceContext? context = GetResourceContext(name);
        if (context != null)
        {
            context.Unload();
            return _contextsByPriority.Remove(context.Priority);
        }

        return false;
    }

    /// <summary>
    /// Removes a resource context by priority.
    /// </summary>
    public bool RemoveResourceContext(int priority)
    {
        if (_contextsByPriority.TryGetValue(priority, out ResourceContext? context))
        {
            context.Unload();
            return _contextsByPriority.Remove(priority);
        }

        return false;
    }

    /// <summary>
    /// Enables a resource context by name.
    /// </summary>
    public void EnableContext(string name)
        => GetResourceContext(name)?.Enable();

    /// <summary>
    /// Enables a resource context by priority.
    /// </summary>
    public void EnableContext(int priority)
        => GetResourceContext(priority)?.Enable();

    /// <summary>
    /// Disables a resource context by name.
    /// </summary>
    public void DisableContext(string name)
        => GetResourceContext(name)?.Disable();

    /// <summary>
    /// Disables a resource context by priority.
    /// </summary>
    public void DisableContext(int priority)
        => GetResourceContext(priority)?.Disable();

    /// <summary>
    /// Removes all resource contexts from the workspace.
    /// </summary>
    public void ClearAllContexts()
    {
        foreach (ResourceContext? context in _contextsByPriority.Values)
            context.Unload();
        _contextsByPriority.Clear();
    }

    #endregion

    #region Version 1: Game Folder Mounting

    /// <summary>
    /// Mounts a game folder as a resource context with the specified priority.
    /// For Version 1 games, use priority 0 for main root, 10 for patch root, etc.
    /// </summary>
    public ResourceContext MountGameFolder(string path, int priority)
    {
        var context = new ResourceContext($"Folder:{Path.GetFileName(path)}", priority, this);
        var folderProvider = new FolderProvider(path, this);

        // GameFolderProvider already loads archives via Workspace reference
        context.AddProvider(folderProvider);

        _contextsByPriority[priority] = context;
        return context;
    }

    #endregion

    #region Version 2: Resource Description Loading

    /// <summary>
    /// Loads a resource description (Lua script) and creates a context from it.
    /// </summary>
    public ResourceContext LoadResourceDescription(string descPath)
    {
        (int priority, string name, List<string> archivePaths) = ParseResourceDescription(descPath);
        var context = new ResourceContext(name, priority, this);

        // Add archives from the resource description
        foreach (string? archivePath in archivePaths)
        {
            string? fullPath = Path.IsPathRooted(archivePath)
                ? archivePath
                : Path.Combine(Path.GetDirectoryName(descPath)!, archivePath);

            var archiveProvider = new ArchiveProvider(
                fullPath,
                this
            );
            context.AddProvider(archiveProvider);
        }

        // Add loose files from the base folder
        string? baseFolder = Path.GetDirectoryName(descPath);
        if (baseFolder != null)
        {
            foreach (string file in Directory.GetFiles(baseFolder))
            {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                if (ext != ".ttarch" && ext != ".ttarch2" && ext != ".lua")
                    context.AddProvider(new LooseFileProvider(file));
            }
        }

        _contextsByPriority[priority] = context;
        return context;
    }

    // TODO: Implement actual Lua parsing
    private (int priority, string name, List<string> archives) ParseResourceDescription(string descPath)
    {
        return (
            priority: 100,
            name: Path.GetFileNameWithoutExtension(descPath),
            archives: new List<string>()
        );
    }

    #endregion

    #region File Operations

    public Stream? ExtractFile(ulong crc64)
    {
        // Search from highest priority to lowest (so highest overrides)
        foreach (KeyValuePair<int, ResourceContext> kvp in _contextsByPriority.Reverse())
        {
            Stream? stream = kvp.Value.ExtractFile(crc64);
            if (stream != null)
                return stream;
        }

        return null;
    }

    public bool ContainsFile(ulong crc64)
    {
        return _contextsByPriority.Values
            .Any(c => c.IsEnabled && c.ContainsFile(crc64));
    }

    /// <summary>
    /// Finds a file entry by CRC64.
    /// </summary>
    public TelltaleFileEntry? FindFileEntry(ulong crc64)
    {
        foreach (KeyValuePair<int, ResourceContext> context in _contextsByPriority.Reverse())
        {
            if (context.Value.IsEnabled)
            {
                TelltaleFileEntry? entry = context.Value.GetFileEntry(crc64);
                if (entry != null)
                    return entry;
            }
        }

        return null;
    }

    #endregion

    #region Archive Management

    public ArchiveBase LoadArchive(string archivePath, bool sort = true, bool debugPrint = false)
    {
        // todo
        ArchiveBase archive = _toolkit.LoadArchive(archivePath, BlowfishKey, sort, debugPrint);
        ArchiveLoaded?.Invoke(archive);
        return archive;
    }

    #endregion

    #region Symbol Resolution

    /// <summary>
    /// Resolves a symbol.
    /// </summary>
    public bool ResolveSymbol(Symbol symbol)
    {
        if (symbol == null)
            throw new ArgumentNullException(nameof(symbol));

        if (symbol.HasString())
            return true;

        if (_toolkit.ResolveSymbol(symbol))
        {
            return true;
        }

        if (LocalHashDatabase.ResolveSymbol(symbol))
        {
            return true;
        }

        TelltaleFileEntry? fileEntry = FindFileEntry(symbol.Crc64);
        if (fileEntry?.Name != null)
        {
            symbol.SymbolName = fileEntry.Name;
            return true;
        }

        return false;
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
    
    #endregion

    #region Object Serialization

    /// <summary>
    /// Loads an object using this game's default configuration.
    /// </summary>
    public T LoadObject<T>(string fileName, out MetaStreamConfiguration config) where T : class, new()
    {
        return _toolkit.LoadObject<T>(fileName, out config);
    }

    /// <summary>
    /// Loads an object from a stream using this game's default configuration.
    /// </summary>
    public object LoadObject(Stream stream, out MetaStreamConfiguration config)
    {
        return _toolkit.LoadObject(stream, out config);
    }

    /// <summary>
    /// Loads an object using this game's default configuration.
    /// </summary>
    public object LoadObject(string fileName, out MetaStreamConfiguration config)
    {
        return _toolkit.LoadObject(fileName, out config);
    }

    /// <summary>
    /// Loads an object from a stream using this game's default configuration.
    /// </summary>
    public T LoadObject<T>(Stream stream, out MetaStreamConfiguration config) where T : class, new()
    {
        return _toolkit.LoadObject<T>(stream, out config);
    }

    /// <summary>
    /// Saves an object using this game's default configuration.
    /// </summary>
    public void SaveObject<T>(T obj, string fileName) where T : class, new()
    {
        _toolkit.SaveObject(obj, fileName, DefaultMetaStreamConfig);
    }

    /// <summary>
    /// Saves an object to a stream using this game's default configuration.
    /// </summary>
    public void SaveObject<T>(T obj, Stream stream) where T : class, new()
    {
        _toolkit.SaveObject(obj, stream, DefaultMetaStreamConfig);
    }

    /// <summary>
    /// Saves an object to a stream using this game's default configuration.
    /// </summary>
    public void SaveObject(object obj, string fileName)
    {
        _toolkit.SaveObject(obj, fileName, DefaultMetaStreamConfig);
    }

    /// <summary>
    /// Saves an object to a stream using this game's default configuration.
    /// </summary>
    public void SaveObject(object obj, Stream stream)
    {
        _toolkit.SaveObject(obj, stream, DefaultMetaStreamConfig);
    }

    /// <summary>
    /// Attempts to open and validate a Telltale file, returning configuration if successful.
    /// </summary>
    public bool TryOpenFile(string fileName, out MetaStreamConfiguration? config)
    {
        return _toolkit.TryOpenFile(fileName, out config);
    }

    /// <summary>
    /// Attempts to open and validate a Telltale file from stream, returning configuration if successful.
    /// </summary>
    public bool TryOpenFile(Stream stream, out MetaStreamConfiguration? config)
    {
        return _toolkit.TryOpenFile(stream, out config);
    }

    /// <summary>
    /// Attempts to load an object from a file, returning null if validation fails.
    /// </summary>
    public T? TryOpenObject<T>(string fileName, out MetaStreamConfiguration? config) where T : class, new()
    {
        return _toolkit.TryOpenObject<T>(fileName, out config);
    }

    /// <summary>
    /// Attempts to load an object from a stream, returning null if validation fails.
    /// </summary>
    public T? TryOpenObject<T>(Stream stream, out MetaStreamConfiguration config) where T : class, new()
    {
        return _toolkit.TryOpenObject<T>(stream, out config);
    }

    #endregion
}
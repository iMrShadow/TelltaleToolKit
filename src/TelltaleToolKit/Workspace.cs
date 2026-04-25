using System.Text;
using Lua;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Resource;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit;

public class Workspace
{
    private static readonly List<LuaTable> ExtractedResourceDescriptions = [];
    private readonly List<ResourceContext> _contexts = [];

    private readonly Toolkit _toolkit;

    private LuaState? _resdecLuaState = null;

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
            Workspace = this,
            CanModifySerializedClassesList = true
        };
    }

    /// <summary>
    /// Gets the game profile for this workspace.
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

    public LuaState ResdecLuaState
    {
        get
        {
            if (_resdecLuaState != null)
            {
                return _resdecLuaState;
            }

            _resdecLuaState = LuaState.Create();

            SetupLuaState(_resdecLuaState);

            return _resdecLuaState;
        }
    }

    private ResourceContext? _contextsByPriority(int priority) => _contexts.Find((ctx) => ctx.Priority == priority);

    private void _addResourceToContexts(ResourceContext ctx)
    {
        _contexts.Add(ctx);
        _contexts.Sort((c, c1) => c.Priority.CompareTo(c1.Priority));
    }

    private static void SetupLuaState(LuaState state)
    {
        state.Environment["RegisterSetDescription"] = new LuaFunction((context, ct) =>
        {
            var resdesc = context.GetArgument<LuaTable>(0);

            ExtractedResourceDescriptions.Add(resdesc);

            context.Return();

            return new(0);
        });

        state.Environment["_currentDirectory"] = "";
    }

    /// <summary>
    /// Event raised when an archive is loaded.
    /// </summary>
    public event Action<ArchiveBase> ArchiveLoaded;

    /// <summary>
    /// Event raised when an archive is unloaded.
    /// </summary>
    public event Action<ArchiveBase> ArchiveUnloaded;

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

        _addResourceToContexts(context);
        return context;
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

    /// <summary>
    /// Explicitly create a resource context from a single archive, without a resdesc file
    /// </summary>
    /// <param name="archivePath"></param>
    /// <param name="contextName"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
    public ResourceContext LoadArchive(string archivePath, string contextName, int priority = 1000)
    {

        ResourceContext context = CreateResourceContext(contextName, priority);
        
        var archiveProvider = new ArchiveProvider(
            archivePath,
            this
        );
        
        context.AddProvider(archiveProvider);

        return context;
    }

    #endregion

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
    /// </summary>
    public ResourceContext CreateResourceContext(string name, int priority)
    {
        var context = new ResourceContext(name, priority, this);
        _addResourceToContexts(context); // Dictionary, not list - priority is unique | PRIORITY IS NOT UNIQUE - Gamma
        return context;
    }

    /// <summary>
    /// Gets a resource context by name.
    /// </summary>
    public ResourceContext? GetResourceContext(string name)
        => _contexts.FirstOrDefault(context => context.Name == name);

    /// <summary>
    /// Gets a resource context by priority.
    /// </summary>
    [Obsolete("Priority is not unique, and _contexts should not be searched in by priority.")]
    public ResourceContext? GetResourceContext(int priority)
        => _contextsByPriority(priority);


    /// <summary>
    /// Removes a resource context by name.
    /// </summary>
    public bool RemoveResourceContext(string name)
    {
        ResourceContext? context = GetResourceContext(name);
        if (context != null)
        {
            context.Unload();
            return _contexts.Remove(context);
        }

        return false;
    }

    /// <summary>
    /// Removes a resource context by priority.
    /// </summary>
    [Obsolete("Priority is not unique, and _contexts should not be searched in by priority.")]
    public bool RemoveResourceContext(int priority)
    {
        ResourceContext? ctx = _contextsByPriority(priority);
        if (ctx != null)
        {
            ctx.Unload();
            return _contexts.Remove(ctx);
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
        foreach (ResourceContext? context in _contexts)
            context.Unload();
        _contexts.Clear();
    }

    #endregion

    #region Version 2: Resource Description Loading

    /// <summary>
    /// Loads a resource description (Lua script) and creates a context from it.
    /// </summary>
    public ResourceContext LoadResourceDescription(string descPath)
    {
        LuaTable resdesc = ParseResourceDescription(descPath).Result;

        var name = resdesc["name"].Read<string>();
        var priority = resdesc["priority"].Read<int>();

        var context = new ResourceContext(name, priority, this);

        LuaValue archives = resdesc["gameDataArchives"];
        
        if(archives.TryRead(out LuaTable table))
        {
            _addGameDataArchives(descPath, table, context);
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

        _addResourceToContexts(context);
        return context;
    }

    /// <summary>
    /// Helper function to add all the game data archives in a given game data archives lua table.
    /// </summary>
    /// <param name="descPath">Resource Description Path</param>
    /// <param name="archives">gameDataArchives entry</param>
    /// <param name="context">resource context</param>
    private void _addGameDataArchives(string descPath, LuaTable archives, ResourceContext context)
    {
        // Add archives from the resource description
        foreach (KeyValuePair<LuaValue, LuaValue> kvPair in archives.ToArray())
        {
            var archivePath = kvPair.Value.Read<string>();

            string fullPath = Path.IsPathRooted(archivePath)
                ? archivePath
                : Path.Combine(Path.GetDirectoryName(descPath)!, archivePath);

            var archiveProvider = new ArchiveProvider(
                fullPath,
                this
            );
            context.AddProvider(archiveProvider);
        }
    }

    private const string LEnHeader = "\eLEn";
    private const string LuaHeader = "\eLua";
    private const string LEoHeader = "\eLEo";

    private async Task<LuaTable> ParseResourceDescription(string descPath)
    {
        FileStream luaFile = File.OpenRead(descPath);

        var headerBytes = new byte[4];
        int read = luaFile.Read(headerBytes, 0, 4);

        if (read < 4)
        {
            throw new IOException("Could not read header from lua file");
        }

        string header = Encoding.ASCII.GetString(headerBytes);

        if (header is not LEnHeader and not LEoHeader)
        {
            luaFile.Position = 0;
        }

        var fileBytes = new byte[luaFile.Length - luaFile.Position];
        read = luaFile.Read(fileBytes, 0, fileBytes.Length);

        if (read < fileBytes.Length)
        {
            throw new IOException("Could not read header from lua file");
        }

        var blowfishInstance = new Blowfish(this.Profile.BlowfishKey, 7);

        blowfishInstance.Decipher(fileBytes, fileBytes.Length);

        if (header is LEnHeader)
        {
            // handle lua bytecode, but this shouldn't actually happen in resdesc parsing...
            // byte[] lua = new byte[fileBytes.Length + 4];
            // Array.Copy(Encoding.ASCII.GetBytes(LuaHeader), lua, 4);
            // Array.Copy(fileBytes, lua, fileBytes.Length);

            Console.WriteLine("Got LEn Lua header during ParseResourceDescription, this is likely the result of passing the wrong .lua file as a resdec.");
            throw new ArgumentException("Improperly formatted resdesc file");
        }

        string lua = Encoding.ASCII.GetString(fileBytes);

        await ResdecLuaState.DoStringAsync(lua);

        LuaTable? resdesc = ExtractedResourceDescriptions.Last();

        return resdesc;
    }

    #endregion

    #region File Operations

    public T? LoadAsset<T>(string name) where T : class, new()
    {
        return LoadAsset<T>(Symbol.GetCrc64(name));
    }

    public T? LoadAsset<T>(ulong crc64) where T : class, new()
    {
        // Search from highest priority to lowest (so highest overrides)
        foreach (ResourceContext? context in _contexts.AsReadOnly().Reverse())
        {
            Stream? stream = context.ExtractFile(crc64);
            if (stream == null)
                continue;

            try
            {
                return LoadObject<T>(stream, out MetaStreamConfiguration? _);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        return null;
    }

    public Stream? ExtractFile(string name)
    {
        return ExtractFile(Symbol.GetCrc64(name));
    }

    public Stream? ExtractFile(ulong crc64)
    {
        // Search from highest priority to lowest (so highest overrides)
        foreach (ResourceContext? context in _contexts.AsReadOnly().Reverse())
        {
            Stream? stream = context.ExtractFile(crc64);
            if (stream != null)
                return stream;
        }

        return null;
    }

    public bool ContainsFile(ulong crc64)
    {
        return _contexts
            .Any(c => c.IsEnabled && c.ContainsFile(crc64));
    }

    /// <summary>
    /// Finds a file entry by CRC64.
    /// </summary>
    public TelltaleFileEntry? FindFileEntry(ulong crc64)
    {
        foreach (ResourceContext? context in _contexts.AsReadOnly().Reverse())
        {
            if (context.IsEnabled)
            {
                TelltaleFileEntry? entry = context.GetFileEntry(crc64);
                if (entry != null)
                    return entry;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the file provider for the given resource
    /// </summary>
    /// <param name="crc64">File to search for</param>
    /// <returns>null if no providers contain the file</returns>
    public IFileProvider? GetFileProviderForResource(ulong crc64)
    {
        foreach (ResourceContext? context in _contexts.AsReadOnly().Reverse())
        {
            foreach (IFileProvider? provider in context.Providers)
            {
                if (provider.ContainsFile(crc64))
                {
                    return provider;
                }
            }
        }

        return null;
    }

    #endregion

    #region Symbol Resolution

    /// <summary>
    /// Resolves a symbol.
    /// </summary>
    /// <returns>true if the symbol has been resolved</returns>
    /// <exception cref="ArgumentNullException"> Symbol is null </exception>
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
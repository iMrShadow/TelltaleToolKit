using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit;

public class TTKContext
{
    private static TTKContext? _instance;

    private MetaClassRegistry MetaClassRegistry { get; set; }
    private MetaClassSerializerSelector MetaClassSerializerSelector { get; set; } = new();

    private TTKContext()
    {
        MetaClassRegistry = new MetaClassRegistry();
    }

    private List<GameDescriptor> RegisteredGames { get; set; } = [];
    public GameDescriptor? ActiveGameRegistry { get; set; }

    public static TTKContext Instance() => _instance ??= new TTKContext();

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

    public MetaClass GetMetaClassDescriptionFromActiveGame(Type type)
    {
        if (ActiveGameRegistry is null)
        {
            throw new InvalidOperationException("No active game registry has been set");
        }

        KeyValuePair<MetaClassType, uint>? match =
            ActiveGameRegistry.Classes.FirstOrDefault(tc => tc.Key.LinkingType == type);

        return MetaClassRegistry.GetClass(match.Value.Key, match.Value.Value);
    }

    public bool IsMetaClassDescriptionInCrc32ActiveGame(MetaClass? desc)
        => ActiveGameRegistry is not null && desc is not null && ActiveGameRegistry.Classes.ContainsKey(desc.ClassType);


    public MetaClass? GetMetaClassDescriptionFromActiveGame(Symbol symbol)
    {
        ArgumentNullException.ThrowIfNull(ActiveGameRegistry);

        KeyValuePair<MetaClassType, uint> match = ActiveGameRegistry.Classes
            .FirstOrDefault(tc => tc.Key.Symbol.Crc64 == symbol.Crc64);

        if (match.Key == null)
            return null;

        return MetaClassRegistry.GetClass(match.Key, match.Value);
    }

    public GameDescriptor GetTelltaleGame(string gameName)
    {
        ArgumentNullException.ThrowIfNull(gameName, nameof(gameName));

        return RegisteredGames.First(g => g.Name.Equals(gameName, StringComparison.OrdinalIgnoreCase));
    }

    public void SetActiveGame(string gameName)
    {
        ArgumentNullException.ThrowIfNull(gameName, nameof(gameName));
        ActiveGameRegistry = FindGame(gameName);
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
}
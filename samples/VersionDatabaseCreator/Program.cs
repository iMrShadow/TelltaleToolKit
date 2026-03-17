using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using TelltaleToolKit;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility.Blowfish;

// !!!IMPORTANT!!!
// This project creates ONLY the VERSION DATABASE for a specific game.
// This project requires to load the existing database for best results.
// When run, it will create the following 3 files:
// 1. The normal -NEW.vdb.json file which will contain key-value pairs - the type name and its corresponding CRC32.
// 2. The -UNSUPPORTED.vdb.json file which will contain metaclass descriptions with no members.
// It acts as a template which has to be filled by hand. Then those metaclass descriptions have to be added in global.vdb.json.
// 3. The -UNSUPPORTED-TYPES.csv file will contain unregistered types with their CRC32.
// If you are comfortable inspecting raw binary data, I suggest looking for the types in a .dmp file searching for "class " or "struct " using ImHex, hexed.it or similar tool.

// Note: types are different from metaclass descriptions.
// A type in this case is just the type itself like "class Vector3", "class D3DMesh", "class Font" etc.
// A metaclass description is a description which describes that type: type members, flags, etc.
// E.g. the description of "class Vector3" will have "X", "Y" and "Z" as members.

// Some games have been released multiple times.
const string sluggifiedName = "back-to-the-future-the-game-episode-1";
const string gameFolderPath = "REPLACE_ME_WITH_GAME_FOLDER_PATH";
const T3BlowfishKey blowfishKey = T3BlowfishKey.Bttf101;

// Alternatively, replace with a full path.
const string dataFolderPath = "../../../../../data";
Toolkit.Initialize(new Toolkit.Configuration()
{
    DataFolder = dataFolderPath,
    JsonOptions = new JsonSerializerOptions()
    {
        Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    }
});

// Replace *ttarch with *ttarch2 if you want to scan ttarch2.
const string filter = "*ttarch";
string[] archivePaths = Directory.GetFiles(gameFolderPath, filter, SearchOption.AllDirectories);

// Think of these as sets, which are thread-safe.
// These are used to collect metastream data in order to write the debug files.
ConcurrentDictionary<MetaClass, byte> serializedMetaClasses = [];
ConcurrentDictionary<(MetaClassType, uint crc32), byte> unrecognizedMetaClassDescriptions = [];
ConcurrentDictionary<(ulong typeHash, uint crc32), byte> unregisteredTypes = [];

// This scans asynchronously (parallel). It's powerful, but somewhat resource heavy, especially for old computers.
// My SSD (Crucial MX500) managed to get up to 3000MB/s read.
// The reason being is that when extracting a single entry directly from a ttarch, that entry is directly loaded in memory.
// And sometimes...there are huge assets.
// It takes a couple of minutes to scan TWD:DE with a stock Ryzen 5 2600.
var areSymbolsHashed = false;
var msv = MetaStreamVersion.Mbin;
ArchiveVersion ttarchVersion = 0;
await Parallel.ForEachAsync(archivePaths, async (filePath, _) =>
{
    Console.WriteLine($"Start reading {filePath}.");

    try
    {
        using ArchiveBase archive = Toolkit.Instance.Load(filePath, blowfishKey, false);
        ttarchVersion = archive.Info.Version;

        if (archive.FileEntries.Length > 0)
        {
            MemoryStream firstFile = archive.ExtractFile(archive.FileEntries[0].Name);

            if (Toolkit.IsMetaFile(archive.FileEntries[0].Name))
            {
                MetaStreamConfiguration config = new MetaStreamReader(firstFile).Configuration;

                areSymbolsHashed = config.AreSymbolsHashed;
                msv = config.Version;
            }
        }

        foreach (TelltaleFileEntry entry in archive.FileEntries)
        {
            // Console.WriteLine($"Reading {entry.Name}");
            if (!Toolkit.IsMetaFile(entry.Name))
                continue;

            MemoryStream file = archive.ExtractFile(entry.Name);

            MetaStreamConfiguration config = new MetaStreamReader(file).Configuration;

            foreach (MetaClass desc in config.SerializedClasses)
                serializedMetaClasses.TryAdd(desc, 0);

            foreach ((ulong, uint) type in config.UnregisteredTypes)
                unregisteredTypes.TryAdd(type, 0);

            foreach ((MetaClassType, uint crc32) urDesc in config.UnregisteredClasses)
                unrecognizedMetaClassDescriptions.TryAdd(urDesc, 0);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Something unexpected happened reading {filePath}!");
        Console.WriteLine(e);
    }

    Console.WriteLine($"Finished reading {filePath}.");

    await Task.CompletedTask;
});

// If you wish to prefer synchronous reading, this will do the job as well.
// foreach (string filePath in archivePaths)
// {
//     Console.WriteLine($"Start reading {filePath}.");
//
//     try
//     {
//         using ArchiveBase archive = TTK.Load(filePath, blowfishKey, false);
//
//         foreach (TelltaleFileEntry entry in archive.FileEntries)
//         {
//
//             // Console.WriteLine($"Reading {entry.Name}");
//             if (!TTK.IsMetaFile(entry.Name))
//             {
//                 continue;
//             }
//
//             MetaStreamConfiguration config = TTK.ExtractMetaStreamConfiguration(archive.ExtractFile(entry.Name));
//
//             // Use this if you want to debug which file has an unknown type.
//             if (config.UnregisteredTypes.Count > 0)
//             {
//                 Console.WriteLine($"File {entry.Name} has an unknown type!");
//                 break;
//             }
//
//             foreach (MetaClass desc in config.SerializedClasses)
//                 serializedMetaClasses.TryAdd(desc, 0);
//
//             foreach ((ulong, uint) type in config.UnregisteredTypes)
//                 unregisteredTypes.TryAdd(type, 0);
//
//             foreach ((MetaClassType, uint crc32) urDesc in config.UnregisteredClasses)
//                 unrecognizedMetaClassDescriptions.TryAdd(urDesc, 0);
//         }
//     }
//     catch (Exception e)
//     {
//         Console.WriteLine($"Something unexpected happened reading {filePath}!");
//         Console.WriteLine(e);
//     }
//
//     Console.WriteLine($"Finished reading {filePath}.");
// }

// Make metaclass descriptions for the unregistered classes.
// Then write them in [name]-UNSUPPORTED-vdb.json
List<MetaClass> unregisteredMetaClasses = [];

foreach ((MetaClassType classType, uint crc32) unrecognized in unrecognizedMetaClassDescriptions.Keys)
{
    var mcs = new MetaClass()
    {
        ClassType = unrecognized.classType, Crc32 = unrecognized.crc32, Members = []
    };
    unregisteredMetaClasses.Add(mcs);
}

IEnumerable<MetaClass> unionized = serializedMetaClasses.Keys.Union(unregisteredMetaClasses);

ImmutableSortedDictionary<string?, uint> classes = unionized.ToDictionary(
    kvp => kvp.ClassType.Symbol.SymbolName,
    kvp => kvp.Crc32
).ToImmutableSortedDictionary();

string database = JsonSerializer.Serialize(classes, Toolkit.Instance.Config.JsonOptions);

// Change output locations if needed.
File.WriteAllText(Path.Join(dataFolderPath, "versiondb", sluggifiedName + "-NEW.vdb.json"), database);

if (unregisteredMetaClasses.Count > 0)
{
    Toolkit.Instance.DumpMetaClassDescriptions(unregisteredMetaClasses,
        Path.Join(dataFolderPath, "versiondb", sluggifiedName + "-UNSUPPORTED.vdb.json"));
}

// All registered types that I currently have can be found in MetaClassTypeRegistry.cs.
// Unsupported types are rare, at least those which are added in the meta headers (and those are the most important ones)
// I added the last types from TWD:DE, which means everything can be read. (Yay!)
// Verified for Back to the Future and Jurassic Park too.
// Even though I am pretty sure I have registered every single type, there are probably some still remaining ones.
// The most common ones should be ScriptEnums (ScriptEnum:[Insert random string]).
// ScriptEnums are registered during RUNTIME using the `ScriptEnumSetValues` lua function.
// They function like...enums, the difference being they are just registered in Lua.
// Example: In CSI3DoM, the enums are defined in CSI3system.lua.
// In the future, I will remove the "UNSUPPORTED-TYPES", because:
// 1. I assume all types will be registered relatively soon.
// 2. It's going to make the code more maintainable.
// Update: As it stands - there are a couple of unregistered types for MCSM and Borderlands.

if (!unregisteredTypes.IsEmpty)
{
    string path = Path.Join(dataFolderPath, "versiondb", sluggifiedName + "-UNSUPPORTED-TYPES.csv");
    IEnumerable<string> lines = unregisteredTypes.Keys.Select(t => $"{t.typeHash},{t.crc32}");
    File.WriteAllLines(path, lines);
}

string jsonFilePath = Path.Combine(dataFolderPath, "game_profiles", sluggifiedName + ".json");
if (!File.Exists(jsonFilePath))
{
    JsonSerializerOptions jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() }
    };

    // Approximate game profile values.
    var gameProfile = new GameProfile
    {
        Id = sluggifiedName,
        Name = sluggifiedName,
        AreSymbolsHashed = areSymbolsHashed,
        BlowfishKey = blowfishKey.ToString(),
        EnableOodleCompression = false,
        IsTtarch2 = filter == "*.ttarch2",
        LuaVersion = LuaVersion.Lua502, // Determining lua versions is too much for me, sorry.
        MetaStreamVersion = msv,
        TtarchVersion = ttarchVersion,
    };

    string json = JsonSerializer.Serialize(gameProfile, jsonOptions);
    File.WriteAllText(jsonFilePath, json, Encoding.ASCII);
}
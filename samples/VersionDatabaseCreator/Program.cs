using System.Collections.Concurrent;
using TelltaleToolKit;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility;

// !!!IMPORTANT!!!
// This project creates ONLY the VERSION DATABASE for a specific game.
// This project requires to load the existing database for best results.
// When run, it will create the following 3 files:
// 1. The normal .vdb.json file which will contain metaclass descriptions for the specific game. (You can check out the already existing ones.)
// 2. The -UNSUPPORTED.vdb.json file which will contain metaclass descriptions with no members. 
// It acts as a template which has to be filled by hand. Then those metaclass descriptions have to be added in the main vdb.json file.
// 3. The -UNSUPPORTED-TYPES.csv file will contain unregistered types with their CRC32.
// If you are comfortable inspecting raw binary  data, I suggest looking for the types in a dmp file searching for "class " or "struct " using ImHex, hexed.it or similar tool.

// Note: types are different from metaclass descriptions.
// A type in this case is just the type itself like "class Vector3", "class D3DMesh", "class Font" etc.
// A metaclass description is a description which describes that type.
// For e.g. the description of "class Vector3" will have X, Y and Z as members.

// Some games have been released multiple times.
const string sluggifiedName = "back-to-the-future-the-game-episode-1";
const string gameFolderPath = "REPLACE_ME_WITH_GAME_FOLDER_PATH";
const T3BlowfishKey blowfishKey = T3BlowfishKey.Bttf101;

// Alternatively, replace with a full path.
const string dataFolderPath = "../../../../../data";
TTKContext.Instance().Load(dataFolderPath);

// Print all registered types if you want to.
// TTKContext.Instance().PrintRegisteredTypes();

// Replace *ttarch with *ttarch2 if you want to scan ttarch2.
string[] archivePaths = Directory.GetFiles(gameFolderPath, "*.ttarch", SearchOption.AllDirectories);

// Think of these as sets, which are thread-safe.
// These are used to collect metastream data in order to write the debug files.
ConcurrentDictionary<MetaClass, byte> serializedMetaClasses = [];
ConcurrentDictionary<(MetaClassType, uint crc32), byte> unrecognizedMetaClassDescriptions = [];
ConcurrentDictionary<(ulong typeHash, uint crc32), byte> unregisteredTypes = [];

// This scans asynchronously (multithreading). It's powerful, but somewhat resource heavy, especially for old computers.
// My SSD (Crucial MX500) managed to get up to 3000MB/s read.
// The reason being is that when extracting a single entry directly from a ttarch, that entry is directly loaded in memory.
// And sometimes...there are huge assets.
// It takes a couple of minutes to scan TWD:DE with a stock Ryzen 5 2600.
await Parallel.ForEachAsync(archivePaths, async (filePath, _) =>
{
    Console.WriteLine($"Start reading {filePath}.");

    try
    {
        using ArchiveBase archive = TTK.Load(filePath, blowfishKey, false);

        foreach (TelltaleFileEntry entry in archive.FileEntries)
        {
            // Console.WriteLine($"Reading {entry.Name}");
            if (!TTK.IsMetaFile(entry.Name))
                continue;

            MetaStreamConfiguration config = TTK.ExtractMetaStreamConfiguration(archive.ExtractFile(entry.Name));

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
// foreach (string filePath in ttarch2Files)
// {
//     Console.WriteLine($"Start reading {filePath}.");
//     
//     try
//     {
//         using Archive archive = TTK.Load(filePath, blowfishKey, false);
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
//             foreach (MetaClass desc in config.SerializedClasses)
//                 serializedMetaClasses.TryAdd(desc, 0);
//
//             foreach ((ulong, uint) type in config.UnregisteredTypes)
//                 unregisteredTypes.TryAdd(type, 0);
//             // Use this if you want to debug which file has an unknown type.
//             if (config.UnregisteredTypes.Count > 0)
//             {
//                 Console.WriteLine($"File {entry.Name} has an unknown type!");
//                 break;
//             }
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
// Or copy-paste from other vdb.json files and edit some fields. :)
List<MetaClass> unregisteredMetaClasses = [];

foreach ((MetaClassType classType, uint crc32) unrecognized in unrecognizedMetaClassDescriptions.Keys)
{
    var mcs = new MetaClass()
    {
        ClassType = unrecognized.classType, Crc32 = unrecognized.crc32, Members = []
    };
    unregisteredMetaClasses.Add(mcs);
}


// Change output locations if needed.
TTKContext.DumpMetaClassDescriptions(serializedMetaClasses.Keys,
    Path.Join(dataFolderPath, "versiondb", sluggifiedName + "-NEW.vdb.json"));

if (unregisteredMetaClasses.Count > 0)
{
    TTKContext.DumpMetaClassDescriptions(unregisteredMetaClasses,
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

if (!unregisteredTypes.IsEmpty)
{
    string path = Path.Join(dataFolderPath, "versiondb", sluggifiedName + "-UNSUPPORTED-TYPES.csv");
    IEnumerable<string> lines = unregisteredTypes.Keys.Select(t => $"{t.typeHash},{t.crc32}");
    File.WriteAllLines(path, lines);
}
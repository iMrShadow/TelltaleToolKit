using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.CommandLine;
using System.Drawing;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using TelltaleToolKit;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.IO.Archives;
using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Utility.Blowfish;
using TelltaleToolKit.Utility.Lua;

namespace DatabaseGenerator;

internal static class Program
{
    static Lock ConsoleLock = new();

    static async Task<int> Main(string[] args)
    {
        var slugOpt = new Option<string>("--slug")
        {
            Required = true,
            Description =
                "Unique identifier for the game (used for output file names, e.g. 'the-walking-dead-season-2').",
        };
        var gamePathOpt = new Option<string>("--game-path")
        {
            Required = true,
            Description =
                "Path to the game's archive folder (typically the 'Pack' directory containing .ttarch/.ttarch2 files).",
        };
        var dataPathOpt = new Option<string>("--data-path")
        {
            DefaultValueFactory = _ => "data",
            Description =
                "Output directory where generated files (versiondb and game_profiles) will be written. Defaults to a relative data folder."
        };

        var filterOpt = new Option<string?>("--filter")
        {
            Description =
                "Archive file filter. Use '*ttarch' or '*ttarch2'. If omitted, the tool will auto-detect based on the folder contents."
        };

        var keyOpt = new Option<T3BlowfishKey?>("--key")
        {
            Description =
                "Predefined Blowfish key enum value for known Telltale games (e.g. Twds2, Batman1, Borderlands)."
        };

        var customKeyOpt = new Option<string?>("--custom-key")
        {
            Description =
                "Custom Blowfish key in hexadecimal format (overrides --key). Use this for unsupported or unknown games. " +
                "Provide as continuous hex string (e.g. \"96CA999F8DDA9A87D7CDD9966295AAB8D59596E5A4B99BD0C9539F8590CDCD9FC8B39993C6C49D9EA5A4CFCDA39DBBDDACA78B94D4A46F\" which is the key for \"The Walking Dead Season 2\".",
        };

        var maxDegreeOpt = new Option<int>("--max-degree")
        {
            Description =
                "Maximum number of archives processed in parallel. Lower this value (1-2) for HDDs to avoid heavy disk thrashing.",
            DefaultValueFactory = _ => Environment.ProcessorCount
        };

        var logFileOpt = new Option<string?>("--log-file")
        {
            Description = "Optional path to a log file where warnings, errors, and scan details will be written."
        };

        var root = new RootCommand("TelltaleToolKit Database Generator v0.1.0")
        {
            slugOpt,
            gamePathOpt,
            dataPathOpt,
            filterOpt,
            keyOpt,
            customKeyOpt,
            maxDegreeOpt,
            logFileOpt
        };

        root.SetAction(async (result, ct) =>
        {
            string slug = result.GetValue(slugOpt)!;
            string gamePath = result.GetValue(gamePathOpt)!;
            string dataPath = result.GetValue(dataPathOpt)!;
            string? filter = result.GetValue(filterOpt);
            T3BlowfishKey? key = result.GetValue(keyOpt);
            string? customKey = result.GetValue(customKeyOpt);
            int maxDegree = result.GetValue(maxDegreeOpt);
            string? logFile = result.GetValue(logFileOpt);

            await using StreamWriter? logWriter = logFile != null ? new StreamWriter(logFile, false) : null;

            if (key == null && customKey == null)
            {
                PrintError("Provide --key or --custom-key", logWriter);
                return;
            }

            Toolkit.Initialize(new Toolkit.Configuration { DataFolder = dataPath });

            // Auto-detect ttarch version.
            string selectedFilter = filter ?? DetectFilter(gamePath);

            PrintInfo($"Using filter: {selectedFilter}");
            Log($"Using filter: {selectedFilter}");

            if (customKey != null)
            {
                PrintInfo($"Using custom key: {customKey}");
                Log($"Using custom key: {customKey}");

                var keyBytes = new byte[customKey.Length / 2];
                for (var i = 0; i < customKey.Length; i += 2)
                    keyBytes[i / 2] = Convert.ToByte(customKey.Substring(i, 2), 16);

                customKey = Encoding.GetEncoding("ISO-8859-1").GetString(keyBytes);
            }
            else if (key != null)
            {
                PrintInfo($"Using key: {key.Value} with value {key.Value.GetBlowfishKey()}");
                Log($"Using key: {key.Value} with value {key.Value.GetBlowfishKey()}");
            }

            string[] archivePaths = Directory.GetFiles(gamePath, selectedFilter, SearchOption.AllDirectories);

            PrintInfo($"Found {archivePaths.Length} archives\n");

            var serializedMetaClasses = new ConcurrentDictionary<MetaClass, byte>();
            var unrecognizedMeta = new ConcurrentDictionary<(MetaClassType, uint), byte>();
            var unregisteredTypes = new ConcurrentDictionary<(ulong, uint), byte>();

            var areSymbolsHashed = true;
            uint msv = 3;
            TTArchiveVersion ttarchVersion = 0;

            var processed = 0;
            int total = archivePaths.Length;

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegree, CancellationToken = ct };

            await Parallel.ForEachAsync(archivePaths, parallelOptions, async (filePath, _) =>
            {
                try
                {
                    // PrintInfo($"Processing... {Path.GetFileName(filePath)}");
                    Log($"Reading {filePath}");

                    using Archive archive = customKey != null
                        ? Toolkit.Instance.LoadArchive(filePath, customKey)
                        : Toolkit.Instance.LoadArchive(filePath, key!.Value);

                    ttarchVersion = archive.Info.Version;

                    if (archive.Entries.Count > 0)
                    {
                        await using Stream? firstFile = archive.OpenResource(archive.GetAllEntries().First().Name);

                        if (Toolkit.IsMetaFile(archive.GetAllEntries().First().Name))
                        {
                            MetaStreamParams config = new BinaryMetaStreamReader(firstFile, null).Params;
                            msv = config.StreamVersion;
                        }
                    }

                    foreach (ResourceEntry entry in archive.Entries.Values)
                    {
                        await using Stream? file = archive.OpenResource(entry.Name);

                        if (!Toolkit.IsMetaFile(file))
                            continue;

                        MetaStreamParams config = new BinaryMetaStreamReader(file, null).Params;

                        foreach (MetaClass desc in config.GetRegisteredClasses())
                            serializedMetaClasses.TryAdd(desc, 0);

                        var localUnregisteredTypes = config.GetUnregisteredTypes();
                        var localUnregisteredClasses = config.GetUnregisteredClasses();

                        foreach ((ulong, uint) t in localUnregisteredTypes)
                            unregisteredTypes.TryAdd(t, 0);

                        foreach ((MetaClassType, uint crc32) c in localUnregisteredClasses)
                            unrecognizedMeta.TryAdd(c, 0);

                        if (localUnregisteredTypes.Count > 0)
                            PrintWarning($"{entry.Name} -> {localUnregisteredTypes.Count} unknown types", logWriter);

                        if (localUnregisteredClasses.Count > 0)
                            PrintWarning($"{entry.Name} -> {localUnregisteredClasses.Count} unknown classes",
                                logWriter);
                    }
                }
                catch (Exception ex)
                {
                    PrintError($"Could not read {filePath}", logWriter);
                    Log(ex.ToString());
                }

                int current = Interlocked.Increment(ref processed);
                DrawProgress(Path.GetFileName(filePath), current, total);

                await Task.CompletedTask;
            });

            Console.WriteLine("\nDone.");

            // Build output files
            PrintInfo("Building output files...");

            // Convert unrecognized class descriptions
            List<MetaClass> unregisteredMetaClasses = [];
            unregisteredMetaClasses.AddRange(unrecognizedMeta.Keys.Select(unrecognized =>
                new MetaClass { ClassType = unrecognized.Item1, Crc32 = unrecognized.Item2, Members = [] }));

            // Merge known + unknown
            IEnumerable<MetaClass> unionized = serializedMetaClasses.Keys.Union(unregisteredMetaClasses);

            // Build dictionary
            ImmutableSortedDictionary<string?, uint> classes = unionized
                .ToDictionary(
                    kvp => kvp.ClassType.Symbol.DebugString,
                    kvp => kvp.Crc32
                )
                .ToImmutableSortedDictionary();

            // Ensure folders exist
            string versionDbPath = Path.Join(dataPath, "versiondb");
            Directory.CreateDirectory(versionDbPath);

            // Serialize database
            string database = JsonSerializer.Serialize(classes, Toolkit.Instance.Config.JsonOptions);

            // Write main DB
            string newDbPath = Path.Join(versionDbPath, $"{slug}-NEW.vdb.json");
            await File.WriteAllTextAsync(newDbPath, database, ct);

            PrintInfo($"Created: {newDbPath}");

            // Write unsupported metaclasses
            if (unregisteredMetaClasses.Count > 0)
            {
                string unsupportedPath = Path.Join(versionDbPath, $"{slug}-UNSUPPORTED.vdb.json");

                Toolkit.Instance.ExportMetaClassDescriptions(unregisteredMetaClasses, unsupportedPath);

                PrintWarning($"Unsupported metaclasses written: {unsupportedPath}", logWriter);
            }

            // Write unsupported types CSV
            if (!unregisteredTypes.IsEmpty)
            {
                string csvPath = Path.Join(versionDbPath, $"{slug}-UNSUPPORTED-TYPES.csv");

                IEnumerable<string> lines = unregisteredTypes.Keys
                    .Select(t => $"{t.Item1},{t.Item2}");

                await File.WriteAllLinesAsync(csvPath, lines, ct);

                PrintWarning($"Unsupported types CSV written: {csvPath}", logWriter);
            }

            // =======================
            // GAME PROFILE GENERATION
            // =======================

            string profilePath = Path.Combine(dataPath, "game_profiles", $"{slug}.json");
            Directory.CreateDirectory(Path.GetDirectoryName(profilePath)!);

            if (!File.Exists(profilePath))
            {
                JsonSerializerOptions jsonOptions = new()
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Converters = { new MetaClassJsonConverter(), new GameRegistryJsonConverter() }
                };

                var gameProfile = new GameProfile
                {
                    Id = slug,
                    Name = slug,
                    BlowfishKey = (key.ToString() ?? customKey) ?? "UNKNOWN",
                    EnableOodleCompression = false,
                    IsTtarch2 = selectedFilter == "*ttarch2",
                    LuaVersion = LuaVersion.Lua512,
                    StreamVersion = msv,
                    TtarchVersion = ttarchVersion,
                };

                string json = JsonSerializer.Serialize(gameProfile, jsonOptions);

                await File.WriteAllTextAsync(profilePath, json, Encoding.ASCII, ct);

                PrintInfo($"Created profile: {profilePath}");
            }
            else
            {
                PrintWarning($"Profile already exists: {profilePath}", logWriter);
            }

            PrintInfo("All files generated successfully.");
            return;

            void Log(string msg)
            {
                logWriter?.WriteLine(msg);
                logWriter?.Flush();
            }
        });

        return await root.Parse(args).InvokeAsync();
    }

    private static string DetectFilter(string path)
    {
        return Directory.GetFiles(path, "*ttarch2", SearchOption.AllDirectories).Length > 0
            ? "*ttarch2"
            : "*ttarch";
    }

    private static void PrintInfo(string msg)
    {
        lock (ConsoleLock)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
            Console.ForegroundColor = c;
        }
    }

    private static void PrintWarning(string msg, StreamWriter? log)
    {
        lock (ConsoleLock)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN] " + msg);
            Console.ForegroundColor = c;
        }

        log?.WriteLine("[WARN] " + msg);
    }

    private static void PrintError(string msg, StreamWriter? log)
    {
        lock (ConsoleLock)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + msg);
            Console.ForegroundColor = c;
        }

        log?.WriteLine("[ERROR] " + msg);
    }

    private static void DrawProgress(string name, int current, int total)
    {
        lock (ConsoleLock)
        {
            const int width = 30;
            double ratio = (double)current / total;
            var filled = (int)(ratio * width);

            string bar = "[" + new string('#', filled) + new string('-', width - filled) + "]";

            Console.WriteLine($"\r{bar} [{current}/{total}] | {name}", Color.Green);
            Console.ResetColor();
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility;

namespace TelltaleToolKit;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Provides static convenience methods for loading, saving, and analyzing data,
/// including MetaStreams, archives, and utility functions.
/// </summary>
public static class TTK
{
    /// <summary>
    /// Loads and deserializes an object of type <typeparamref name="T"/> from the specified file,
    /// returning the loaded object and the associated <see cref="MetaStreamConfiguration"/>.
    /// </summary>
    /// <typeparam name="T">The type to deserialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="fileName">The path to the file to load from.</param>
    /// <param name="configuration">Outputs the <see cref="MetaStreamConfiguration"/> used during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="Exception">Thrown if the object cannot be initialized.</exception>
    public static T Load<T>(string fileName, out MetaStreamConfiguration configuration) where T : class, new()
        => Load<T>(File.OpenRead(fileName), out configuration);

    /// <summary>
    /// Loads and deserializes an object of type <typeparamref name="T"/> from the specified stream,
    /// returning the loaded object and the associated <see cref="MetaStreamConfiguration"/>.
    /// </summary>
    /// <typeparam name="T">The type to deserialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="configuration">Outputs the <see cref="MetaStreamConfiguration"/> used during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="Exception">Thrown if the object cannot be initialized.</exception>
    public static T Load<T>(Stream stream, out MetaStreamConfiguration configuration) where T : class, new()
    {
        var streamReader = new MetaStreamReader(stream);

        var type = default(T);

        streamReader.Serialize(ref type);

        if (type is null)
        {
            throw new InvalidOperationException(".NET type cannot be initialized!");
        }

        configuration = streamReader.Configuration;

        return type;
    }

    /// <summary>
    /// Extracts the <see cref="MetaStreamConfiguration"/> from the specified file.
    /// </summary>
    /// <param name="fileName">The path to the file.</param>
    /// <returns>The extracted <see cref="MetaStreamConfiguration"/>.</returns>
    public static MetaStreamConfiguration ExtractMetaStreamConfiguration(string fileName)
        => ExtractMetaStreamConfiguration(File.OpenRead(fileName));

    /// <summary>
    /// Extracts the <see cref="MetaStreamConfiguration"/> from the specified stream.
    /// </summary>
    /// <param name="stream">The stream containing the MetaStream data.</param>
    /// <returns>The extracted <see cref="MetaStreamConfiguration"/>.</returns>
    public static MetaStreamConfiguration ExtractMetaStreamConfiguration(Stream stream)
        => new MetaStreamReader(stream).Configuration;

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified file.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="fileName">The path to the file to save to.</param>
    public static void Save<T>(T obj, string fileName) where T : class, new() =>
        Save(obj, File.OpenWrite(fileName));

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified stream.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="stream">The stream to write to.</param>
    public static void Save<T>(T obj, Stream stream) where T : class, new()
    {
        var streamWriter = new MetaStreamWriter(stream);

        var type = default(T);

        streamWriter.Serialize(ref type);

        if (type is null)
        {
            throw new InvalidOperationException("Type could not be initialized");
        }
    }

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified file using a provided <see cref="MetaStreamConfiguration"/>.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="fileName">The path to the file to save to.</param>
    /// <param name="configuration">The <see cref="MetaStreamConfiguration"/> to use during serialization.</param>
    public static void Save<T>(T obj, string fileName, MetaStreamConfiguration configuration) where T : class, new() =>
        Save(obj, File.OpenWrite(fileName), configuration);

    /// <summary>
    /// Serializes and saves an object of type <typeparamref name="T"/> to the specified stream using a provided <see cref="MetaStreamConfiguration"/>.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must be a class with a parameterless constructor.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="configuration">The <see cref="MetaStreamConfiguration"/> to use during serialization.</param>
    public static void Save<T>(T obj, Stream stream, MetaStreamConfiguration configuration) where T : class, new()
    {
        var streamWriter = new MetaStreamWriter(stream, configuration);
        streamWriter.Serialize(ref obj);
        streamWriter.Save();
    }

    /// <summary>
    /// Performs the first step of serialization or deserialization using the Telltale ToolKit context.
    /// </summary>
    /// <remarks>
    /// Typically, it will instantiate the object if [null], and if it's a collection clear it.
    /// </remarks>
    /// <param name="obj">The object to process.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    /// <param name="type"></param>
    public static void PreSerialize<T>(ref T obj, MetaStream stream, MetaClassType? type = null) where T : new()
    {
        TTKContext.Instance().GetSerializer<T>().PreSerialize(ref obj, stream, type);
    }

    /// <summary>
    /// Serializes or deserializes the given object <paramref name="obj"/> using the Telltale ToolKit context.
    /// </summary>
    /// <param name="obj">The object to serialize or deserialize.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    public static void Serialize<T>(ref T obj, MetaStream stream) where T : new()
    {
        TTKContext.Instance().GetSerializer<T>().Serialize(ref obj, stream);
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
    public static ArchiveBase Load(string ttarch, T3BlowfishKey game, bool sort = true,
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
    /// Determines whether a file with the specified name should be considered a Meta (Telltale Tool specific) file,
    /// based on its extension.
    /// </summary>
    /// <param name="fileName">The name of the file or archive entry.</param>
    /// <returns><c>true</c> if the file is a meta file; otherwise, <c>false</c>.</returns>
    public static bool IsMetaFile(string fileName)
    {
        string extension = Path.GetExtension(fileName);

        return extension switch
        {
            ".lua" or ".lenc" or ".ncb" or ".vws" or ".txt" or ".data" or ".scc" or ".vssscc" or ".dds" or ".sfk"
                or ".xls" or ".suo" or ".vpw" or ".vtg" or ".tga" or ".vers" or ".ttarch" or ".ttarch2" or ".json"
                or ".wav" or ".ogg" or ".html" or ".css" => false,
            _ => true
        };
    }
}
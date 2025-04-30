using System.IO.Compression;
using TelltaleToolKit.Utility;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.TelltaleArchives;

// TODO: Rework the archive system for v0.X.0. Currently it is very messy due to debugging reasons with no documentation.
// All ttarch implementations are based on ttarchext and TTG Tools.
public abstract class ArchiveBase : IDisposable
{
    protected Stream? ArchiveStream { get; set; }
    protected ArchiveInfo Info { get; set; } = new();
    public TelltaleFileEntry[] FileEntries { get; set; } = [];

    public void Dispose()
    {
        ArchiveStream?.Dispose();
        GC.SuppressFinalize(this);
    }

    public static T Load<T>(string filePath, string blowFishKey, bool sortEntries = true, bool debugPrint = false)
        where T : ArchiveBase, new()
    {
        FileStream stream = File.OpenRead(filePath);

        if (debugPrint)
        {
            Console.WriteLine($"Loading {Path.GetFileName(filePath)} from {filePath}");
        }

        return Load<T>(stream, blowFishKey, debugPrint);
    }

    public static T Load<T>(Stream stream, string blowFishKey, bool sortEntries = true, bool debugPrint = false)
        where T : ArchiveBase, new()
    {
        var resource = new T();
        resource.InitializeWithStream(stream);
        resource.Info.BlowfishKey = blowFishKey ?? throw new ArgumentNullException(nameof(blowFishKey));

        resource.ReadMetadata();

        if (sortEntries)
        {
            resource.SortEntries();
        }

        if (debugPrint)
        {
            resource.PrintDebug();
        }

        return resource;
    }

    public static T Load<T>(string filePath, T3BlowfishKey gameKey, bool sortEntries = true, bool debugPrint = false)
        where T : ArchiveBase, new()
    {
        FileStream stream = File.OpenRead(filePath);
        string blowFishKey = gameKey.GetBlowfishKey();
        return Load<T>(stream, blowFishKey, sortEntries, debugPrint);
    }

    public static T Load<T>(Stream stream, T3BlowfishKey gameKey, bool sortEntries = true, bool debugPrint = false)
        where T : ArchiveBase, new()
    {
        var archive = new T();
        archive.InitializeWithStream(stream);
        archive.Info.BlowfishKey = gameKey.GetBlowfishKey();
        archive.ReadMetadata();

        if (sortEntries)
        {
            archive.SortEntries();
        }

        if (debugPrint)
        {
            archive.PrintDebug();
        }

        return archive;
    }

    private void InitializeWithStream(Stream stream)
    {
        ArchiveStream = stream ?? throw new ArgumentNullException(nameof(stream));
        ArchiveStream.Position = 0;
    }

    protected abstract void ReadMetadata();
    public abstract MemoryStream ExtractFile(string fileName);
    public abstract void ExtractAll(string destinationPath);

    protected bool IsEncrypted() =>
        Info.Flags.HasFlag(ContainerFlags.IsEncrypted) ||
        Info.Flags.HasFlag(ContainerFlags.IsModifiedBlowfishEncrypted);

    protected bool IsCompressed() =>
        Info.Flags.HasFlag(ContainerFlags.IsZlibCompressed)
        || Info.Flags.HasFlag(ContainerFlags.IsOodleCompressed)
        || Info.Flags.HasFlag(ContainerFlags.IsRawDeflateCompressed);

    protected int FileCount() => FileEntries.Length;

    protected TelltaleFileEntry? FindEntry(string name) =>
        FileEntries.FirstOrDefault(file => file.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    protected void DecryptBlock(Span<byte> data, int version, string key, ContainerFlags flags)
    {
        if (!IsEncrypted())
        {
            return; // No encryption, return the original data
        }

        Blowfish dec = new(key, version);
        dec.Decipher(data, data.Length);
    }

    public void SortEntries()
        => Array.Sort(FileEntries, (x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));

    public void PrintEntries() => FileEntries.ToList().ForEach(Console.WriteLine);

    public void PrintInfo() =>
        Console.WriteLine(
            $"""
             Container Version: {Info.ContainerVersion}
             Archive Version: {Info.Version}
             Flags: {Info.Flags}
             Chunk Size: {Info.ChunkSize}
             Chunk Count: {Info.ChunkCount}
             File Count: {FileCount()}
             Blowfish Key: {Info.BlowfishKey}
             File Count: {Info.FileCount}
             Is ZlibCompressed: {Info.Flags.HasFlag(ContainerFlags.IsZlibCompressed)}
             Is RawInflate: {Info.Flags.HasFlag(ContainerFlags.IsRawDeflateCompressed)}
             Is Encrypted: {IsEncrypted()}
             """
        );

    private void PrintDebug()
    {
        PrintInfo();
        PrintEntries();
    }
}
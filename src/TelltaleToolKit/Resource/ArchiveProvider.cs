using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit.Resource;

public sealed class ArchiveProvider : IFileProvider, IDisposable
{
    private readonly ArchiveBase _archive;
    private readonly Workspace _workspace;

    public ArchiveProvider(string path, Workspace workspace)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(path);
        _workspace = workspace;
        _archive = workspace.LoadArchive(path);
    }

    public string Name { get; }
    public string Path { get; }

    public void Dispose() => _archive.Dispose();

    public Stream? ExtractFile(ulong crc64) => _archive.ExtractFile(crc64);
    public bool ContainsFile(ulong crc64) => _archive.ContainsFile(crc64);
    public TelltaleFileEntry? GetFileEntry(ulong crc64) => _archive.FindEntry(crc64);

    public Stream? ExtractFile(string fileName)
        => ExtractFile(Crc64.Compute(fileName));

    public bool ContainsFile(string fileName)
        => ContainsFile(Crc64.Compute(fileName));

    public TelltaleFileEntry? GetFileEntry(string fileName)
        => GetFileEntry(Crc64.Compute(fileName));

    public IEnumerable<TelltaleFileEntry> GetAllEntries()
    {
        return _archive.FileEntries;
    }
}

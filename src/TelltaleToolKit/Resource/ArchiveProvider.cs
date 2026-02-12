using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;

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
        => ExtractFile(Symbol.GetCrc64(fileName));
    
    public bool ContainsFile(string fileName) 
        => ContainsFile(Symbol.GetCrc64(fileName));
    
    public TelltaleFileEntry? GetFileEntry(string fileName) 
        => GetFileEntry(Symbol.GetCrc64(fileName));
}
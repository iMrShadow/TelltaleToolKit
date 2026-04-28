using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit.Resource;

public sealed class FolderProvider : IFileProvider
{
    private readonly List<IFileProvider> _providers = new();
    private readonly Workspace _workspace;

    public FolderProvider(string rootPath, Workspace workspace, int priorityRank = 0)
    {
        RootPath = rootPath;
        Name = Path.GetFileName(rootPath);
        PriorityRank = priorityRank;
        _workspace = workspace;

        // 1. Loose files in root (highest priority within this folder)
        foreach (string file in Directory.GetFiles(rootPath))
        {
            string ext = Path.GetExtension(file).ToLowerInvariant();
            if (ext != ".ttarch" && ext != ".ttarch2") 
                _providers.Add(new LooseFileProvider(file));
        }

        // 2. Archives in root
        foreach (string archivePath in Directory.GetFiles(rootPath, "*.ttarch"))
        {
            _providers.Add(new ArchiveProvider(archivePath, workspace));
        }

        // 3. Subfolders (recursively with lower priority)
        foreach (string? subdir in Directory.GetDirectories(rootPath))
        {
            _providers.Add(new FolderProvider(subdir, workspace, priorityRank + 1));
        }
    }

    public string Name { get; }
    public string RootPath { get; }
    public int PriorityRank { get; } // 0 = top folder, 1 = archives, 2 = subfolders, etc.

    public Stream? ExtractFile(ulong crc64)
    {
        foreach (var provider in _providers)
        {
            var stream = provider.ExtractFile(crc64);
            if (stream != null) return stream;
        }
        return null;
    }

    public bool ContainsFile(ulong crc64)
        => _providers.Any(p => p.ContainsFile(crc64));

    public TelltaleFileEntry? GetFileEntry(ulong crc64)
    {
        foreach (var provider in _providers)
        {
            var entry = provider.GetFileEntry(crc64);
            if (entry != null) return entry;
        }
        return null;
    }

    public Stream? ExtractFile(string fileName) 
        => ExtractFile(Symbol.GetCrc64(fileName));
    
    public bool ContainsFile(string fileName) 
        => ContainsFile(Symbol.GetCrc64(fileName));
    
    public TelltaleFileEntry? GetFileEntry(string fileName) 
        => GetFileEntry(Symbol.GetCrc64(fileName));

    public IEnumerable<TelltaleFileEntry> GetAllEntries()
    {
        IEnumerable<TelltaleFileEntry> entries = [];

        return _providers.Aggregate(entries, (current, provider) => current.Concat(provider.GetAllEntries()));
    }
}
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit.Resource;

public class LooseFileProvider : IFileProvider
{
    public LooseFileProvider(string path)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(path);
        Crc64 = Symbol.GetCrc64(Name);
        Size = new FileInfo(path).Length;
    }

    public string Path { get; }
    public string Name { get; }
    public ulong Crc64 { get; }
    public long Size { get; }

    // CRC64 methods
    public Stream? ExtractFile(ulong crc64)
        => crc64 == Crc64 ? File.OpenRead(Path) : null;

    public bool ContainsFile(ulong crc64)
        => crc64 == Crc64;

    public TelltaleFileEntry? GetFileEntry(ulong crc64)
    {
        if (crc64 != Crc64) return null;
        return new TelltaleFileEntry
        {
            Crc64 = crc64,
            Name = Name,
            FileOffset = 0,
            FileSize = (int)Size,
        };
    }

    // Filename methods
    public Stream? ExtractFile(string fileName)
        => fileName == Name ? File.OpenRead(Path) : null;

    public bool ContainsFile(string fileName)
        => fileName == Name;

    public TelltaleFileEntry? GetFileEntry(string fileName)
        => fileName == Name ? GetFileEntry(Crc64) : null;

    public IEnumerable<TelltaleFileEntry> GetAllEntries()
    {
        return
        [
            new TelltaleFileEntry
            {
                Crc64 = Crc64,
                Name = Name,
                FileOffset = 0,
                FileSize = (int)Size,
            }
        ];
    }
}
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit.Resource;

public interface IFileProvider
{
    Stream? ExtractFile(ulong crc64);
    bool ContainsFile(ulong crc64);
    TelltaleFileEntry? GetFileEntry(ulong crc64);

    Stream? ExtractFile(string fileName);
    bool ContainsFile(string fileName);
    TelltaleFileEntry? GetFileEntry(string fileName);

    public IEnumerable<TelltaleFileEntry> GetAllEntries();
}
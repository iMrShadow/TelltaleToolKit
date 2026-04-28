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

    /// <summary>
    /// This method should return an IEnumerable over all TelltaleFileEntries that this IFileProvider can access,
    /// and should not contain duplicates.
    /// 
    /// This should be equivalent to direcly accessing the enumerator for the archives array in
    /// ArchiveProvider or an enumerator containing a single entry for LooseFileProvider.
    /// </summary>
    /// <returns> IEnumerable over file entries accessible to this IFileProvider </returns>
    public IEnumerable<TelltaleFileEntry> GetAllEntries();
}
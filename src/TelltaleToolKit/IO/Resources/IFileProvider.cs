using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit.Resource;

/// <summary>
/// Abstracts access to one or more Telltale game files, whether from a loose file on disk,
/// a packed archive (.ttarch / .ttarch2), or a directory tree of either.
/// </summary>
/// <remarks>
/// All lookup methods accept either a raw CRC-64 hash or a plain filename string.
/// String overloads must hash the name themselves before delegating to the CRC-64 overload,
/// so callers that already have the hash can skip the hashing step.
/// </remarks>
public interface IFileProvider
{
    /// <summary>
    /// Opens a read-only stream for the file whose CRC-64 matches <paramref name="crc64"/>.
    /// </summary>
    /// <param name="crc64">Pre-computed CRC-64 of the filename.</param>
    /// <returns>
    /// A readable, non-seekable stream positioned at the start of the file data,
    /// or <see langword="null"/> if this provider does not contain the file.
    /// The caller is responsible for disposing the stream.
    /// </returns>
    Stream? ExtractFile(ulong crc64);

    /// <summary>
    /// Returns <see langword="true"/> if this provider contains a file whose CRC-64
    /// matches <paramref name="crc64"/>.
    /// </summary>
    /// <param name="crc64">Pre-computed CRC-64 of the filename.</param>
    bool ContainsFile(ulong crc64);

    /// <summary>
    /// Returns the <see cref="ResourceEntry"/> for the file whose CRC-64 matches
    /// <paramref name="crc64"/>, or <see langword="null"/> if not found.
    /// </summary>
    /// <param name="crc64">Pre-computed CRC-64 of the filename.</param>
    ResourceEntry? GetFileEntry(ulong crc64);

    /// <summary>
    /// Opens a read-only stream for the file with the given name.
    /// Implementations should hash <paramref name="fileName"/> with CRC-64 and
    /// delegate to <see cref="ExtractFile(ulong)"/>.
    /// </summary>
    /// <param name="fileName">
    /// The bare filename (e.g. <c>obj_clementine.d3dtx</c>), not a full path.
    /// </param>
    /// <returns>
    /// A readable stream, or <see langword="null"/> if the file is not present.
    /// The caller is responsible for disposing the stream.
    /// </returns>
    Stream? ExtractFile(string fileName);

    /// <inheritdoc cref="ContainsFile(ulong)"/>
    /// <param name="fileName">
    /// The bare filename (e.g. <c>obj_clementine.d3dtx</c>), not a full path.
    /// </param>
    bool ContainsFile(string fileName);

    /// <inheritdoc cref="GetFileEntry(ulong)"/>
    /// <param name="fileName">
    /// The bare filename (e.g. <c>obj_clementine.d3dtx</c>), not a full path.
    /// </param>
    ResourceEntry? GetFileEntry(string fileName);

    /// <summary>
    /// Enumerates every <see cref="ResourceEntry"/> accessible through this provider,
    /// without duplicates.
    /// </summary>
    /// <remarks>
    /// For a <c>LooseFileProvider</c> this yields a single entry.
    /// For an <c>ArchiveProvider</c> this is equivalent to iterating the archive's entry table.
    /// For a <c>FolderProvider</c> or <c>ResourceContext</c> this is the union of all
    /// child providers' entries.
    /// </remarks>
    /// <returns>An enumerable of file entries; never <see langword="null"/>.</returns>
    public IEnumerable<ResourceEntry> GetAllEntries();
}

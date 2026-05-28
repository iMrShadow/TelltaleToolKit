using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit.Resource;

/// <summary>
/// Wraps a single Telltale archive (.ttarch or .ttarch2) as an <see cref="IFileProvider"/>.
/// </summary>
/// <remarks>
/// The underlying <see cref="Archive"/> is loaded through the workspace so that the
/// workspace's <c>ArchiveLoaded</c> event fires and blowfish decryption is applied
/// consistently. Dispose this provider when the archive is no longer needed to release
/// the file handle held by the archive.
/// </remarks>
public sealed class ArchiveProvider : IFileProvider, IDisposable
{
    private readonly Archive _archive;

    /// <summary>
    /// Loads the archive at <paramref name="path"/> using the blowfish key embedded in
    /// <paramref name="workspace"/>.
    /// </summary>
    /// <param name="path">Absolute or relative path to the .ttarch / .ttarch2 file.</param>
    /// <param name="workspace">
    /// The workspace whose <see cref="Workspace.LoadArchive(string, bool, bool)"/> method is
    /// used so the <c>ArchiveLoaded</c> event fires and the correct decryption key is applied.
    /// </param>
    /// <exception cref="FileNotFoundException">
    /// Thrown when <paramref name="path"/> does not point to an existing file.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Thrown when the file extension is not <c>.ttarch</c> or <c>.ttarch2</c>.
    /// </exception>
    public ArchiveProvider(string path, Workspace workspace)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(path);
        _archive = workspace.LoadArchive(path);
    }

    /// <summary>Gets the bare filename of the archive (e.g. <c>WDC_pc_WalkingDead201_data.ttarch2</c>).</summary>
    public string Name { get; }

    /// <summary>Gets the full path to the archive on disk.</summary>
    public string Path { get; }

    /// <summary>
    /// Disposes the underlying <see cref="Archive"/>, releasing any held file handles.
    /// </summary>
    public void Dispose() => _archive.Dispose();

    /// <inheritdoc/>
    public Stream? ExtractFile(ulong crc64) => _archive.OpenResource(crc64);

    /// <inheritdoc/>
    public bool ContainsFile(ulong crc64) => _archive.Entries.ContainsKey(crc64);

    /// <inheritdoc/>
    public ResourceEntry? GetFileEntry(ulong crc64) => _archive.FindResource(crc64);

    /// <inheritdoc/>
    public Stream? ExtractFile(string fileName)
        => ExtractFile(Crc64.Compute(fileName));

    /// <inheritdoc/>
    public bool ContainsFile(string fileName)
        => ContainsFile(Crc64.Compute(fileName));

    /// <inheritdoc/>
    public ResourceEntry? GetFileEntry(string fileName)
        => GetFileEntry(Crc64.Compute(fileName));

    /// <inheritdoc/>
    public IEnumerable<ResourceEntry> GetAllEntries() => _archive.Entries.Values;
}

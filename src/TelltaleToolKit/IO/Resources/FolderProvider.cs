using TelltaleToolKit.IO.Archives;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit.IO.Resources;

/// <summary>
/// Recursively exposes a directory tree of loose files and Telltale archives
/// (.ttarch / .ttarch2) as a single <see cref="IFileProvider"/>.
/// </summary>
/// <remarks>
/// <para>
/// Within each directory level, providers are ordered by priority:
/// <list type="number">
///   <item><description>Loose files in the root directory (highest priority).</description></item>
///   <item><description>Archives (.ttarch and .ttarch2) in the root directory.</description></item>
///   <item><description>Sub-directories, each wrapped in a child <see cref="FolderProvider"/>
///   with an incremented <see cref="PriorityRank"/>.</description></item>
/// </list>
/// </para>
/// <para>
/// File lookups short-circuit on the first provider that contains the requested file,
/// so loose files always shadow archives at the same level.
/// </para>
/// </remarks>
public sealed class FolderProvider : IFileProvider
{
    private static readonly string[] ArchiveExtensions = [".ttarch", ".ttarch2"];

    private readonly List<IFileProvider> _providers = new();

    /// <summary>
    /// Recursively builds the provider hierarchy rooted at <paramref name="rootPath"/>.
    /// </summary>
    /// <param name="rootPath">Absolute path to the root folder.</param>
    /// <param name="workspace">
    /// Workspace used to load archives so that the <c>ArchiveLoaded</c> event fires
    /// and the correct blowfish key is applied.
    /// </param>
    /// <param name="priorityRank">
    /// Depth of this folder relative to the top of the hierarchy.
    /// 0 = top-level folder, 1 = one level down, etc.
    /// Used for informational purposes only; it does not affect lookup order.
    /// </param>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown when <paramref name="rootPath"/> does not exist.
    /// </exception>
    public FolderProvider(string rootPath, Workspace workspace, int priorityRank = 0)
    {
        if (!Directory.Exists(rootPath))
            throw new DirectoryNotFoundException($"Folder not found: {rootPath}");

        RootPath = rootPath;
        Name = Path.GetFileName(rootPath);
        PriorityRank = priorityRank;

        // 1. Loose files in root (highest priority within this folder)
        foreach (string file in Directory.GetFiles(rootPath))
        {
            string ext = Path.GetExtension(file).ToLowerInvariant();
            if (!ArchiveExtensions.Contains(ext))
                _providers.Add(new LooseFileProvider(file));
        }

        // 2. Archives in root
        foreach (string archivePath in Directory.GetFiles(rootPath)
                     .Where(f => ArchiveExtensions.Contains(
                         Path.GetExtension(f).ToLowerInvariant())))
        {
            _providers.Add(new ArchiveProvider(archivePath, workspace));
        }

        // 3. Subfolders (recursively with lower priority)
        foreach (string subdir in Directory.GetDirectories(rootPath))
        {
            _providers.Add(new FolderProvider(subdir, workspace, priorityRank + 1));
        }
    }

    /// <summary>Gets the bare directory name (no path component).</summary>
    public string Name { get; }

    /// <summary>Gets the absolute path to the root of this folder hierarchy.</summary>
    public string RootPath { get; }

    /// <summary>
    /// Gets the depth of this folder relative to the top of the hierarchy.
    /// 0 means this is the top-level folder; higher values are nested sub-directories.
    /// </summary>
    public int PriorityRank { get; }

    /// <inheritdoc/>
    public Stream? ExtractFile(ulong crc64)
    {
        foreach (var provider in _providers)
        {
            var stream = provider.ExtractFile(crc64);
            if (stream != null) return stream;
        }
        return null;
    }

    /// <inheritdoc/>
    public bool ContainsFile(ulong crc64)
        => _providers.Any(p => p.ContainsFile(crc64));

    /// <inheritdoc/>
    public ResourceEntry? GetFileEntry(ulong crc64)
    {
        foreach (var provider in _providers)
        {
            var entry = provider.GetFileEntry(crc64);
            if (entry != null) return entry;
        }
        return null;
    }

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
    public IEnumerable<ResourceEntry> GetAllEntries()
        => _providers.SelectMany(p => p.GetAllEntries());

    /// <summary>
    /// Exposes the direct child providers of this folder for recursive traversal
    /// (e.g. by <see cref="Workspace.GetFileProviderForResource"/>).
    /// </summary>
    internal IReadOnlyList<IFileProvider> GetAllProviders() => _providers;
}

using TelltaleToolKit.IO.Archives;

namespace TelltaleToolKit.IO.Resources;

/// <summary>
/// Exposes a single loose file on disk as an <see cref="IFileProvider"/>.
/// </summary>
/// <remarks>
/// Filename comparisons are case-insensitive so that lookups behave consistently
/// across platforms and match the case-folding convention used by Telltale archives.
/// </remarks>
public class LooseFileProvider : IFileProvider
{
    /// <summary>
    /// Initializes the provider for the file at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">Absolute or relative path to the loose file on disk.</param>
    /// <exception cref="FileNotFoundException">
    /// Thrown when <paramref name="path"/> does not point to an existing file.
    /// </exception>
    public LooseFileProvider(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Loose file not found: {path}", path);

        Path = path;
        Name = System.IO.Path.GetFileName(path);
        Crc64 = Utility.Hashing.Crc64.Compute(Name);
        Size = new FileInfo(path).Length;
    }

    /// <summary>Gets the full path to the file on disk.</summary>
    public string Path { get; }

    /// <summary>Gets the bare filename (no directory component).</summary>
    public string Name { get; }

    /// <summary>Gets the pre-computed CRC-64 of <see cref="Name"/>.</summary>
    public ulong Crc64 { get; }

    /// <summary>Gets the size of the file in bytes.</summary>
    public long Size { get; }

    /// <inheritdoc/>
    public Stream? ExtractFile(ulong crc64)
        => crc64 == Crc64 ? File.OpenRead(Path) : null;

    /// <inheritdoc/>
    public bool ContainsFile(ulong crc64)
        => crc64 == Crc64;

    /// <inheritdoc/>
    public ResourceEntry? GetFileEntry(ulong crc64)
    {
        if (crc64 != Crc64) return null;

        return new ResourceEntry
        {
            NameCrc = crc64, Name = Name, Offset = 0, Size = (uint)Size,
        };
    }

    /// <inheritdoc/>
    public Stream? ExtractFile(string fileName)
        => IsMatch(fileName) ? File.OpenRead(Path) : null;

    /// <inheritdoc/>
    public bool ContainsFile(string fileName)
        => IsMatch(fileName);

    /// <inheritdoc/>
    public ResourceEntry? GetFileEntry(string fileName)
        => IsMatch(fileName) ? GetFileEntry(Crc64) : null;

    /// <inheritdoc/>
    public IEnumerable<ResourceEntry> GetAllEntries()
    {
        yield return new ResourceEntry
        {
            NameCrc = Crc64, Name = Name, Offset = 0, Size = (uint)Size,
        };
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="fileName"/> matches
    /// <see cref="Name"/> under an ordinal, case-insensitive comparison.
    /// </summary>
    private bool IsMatch(string fileName)
        => string.Equals(fileName, Name, StringComparison.OrdinalIgnoreCase);
}

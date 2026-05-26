using System.IO.Enumeration;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives.Formats;
using TelltaleToolKit.Utility.Blowfish;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit.TelltaleArchives;

/// <summary>
///     Base class for all Telltale archive formats (<c>.ttarch</c>, <c>.ttarch2</c>, PK2).
/// </summary>
/// <remarks>
///     <para>
///         Subclasses implement <see cref="Activate" /> to parse their specific header format and
///         populate the entry table via <see cref="SetEntries" />. Extraction is handled by
///         <see cref="OpenResource(ResourceEntry?)" />, which each subclass overrides to return an
///         appropriately positioned stream into the archive data.
///     </para>
///     <para>
///         Entries are keyed by CRC-64 hash of their filename. Use <see cref="FindResource(string)" />
///         for name-based lookups, or <see cref="FindResource(ulong)" /> when the hash is already known.
///     </para>
///     <para>
///         Archives are opened via the static <see cref="Load{T}(string, string)" /> family of methods
///         and created via
///         <see cref="Create{TArchive}(Stream, IEnumerable{ValueTuple{string, Stream}}, ArchiveWriteOptions)" />
///         or the <c>CreateFrom*</c> convenience overloads.
///     </para>
///     <para>
///         Implements <see cref="IDisposable" />. Always dispose the archive (or use a <c>using</c>
///         block) to release the underlying stream.
///     </para>
/// </remarks>
public abstract class Archive : IDisposable
{
    private readonly Dictionary<ulong, ResourceEntry> _entries = new();

    // The raw underlying stream for the archive file. Subclasses read from this directly.
    internal Stream? ArchiveStream { get; set; }

    /// <summary>
    ///     Gets or sets archive metadata, including version, flags, Blowfish key, and chunk layout.
    ///     Populated during <see cref="Activate" />.
    /// </summary>
    public ArchiveInfo Info { get; set; } = new();

    /// <summary>
    ///     Gets a read-only view of all entries loaded from the archive, keyed by CRC-64 filename hash.
    /// </summary>
    public IReadOnlyDictionary<ulong, ResourceEntry> Entries => _entries;

    /// <summary>Number of entries in the archive.</summary>
    public int Count => _entries.Count;

    // -------------------------------------------------------------------------
    // IDisposable
    // -------------------------------------------------------------------------

    public virtual void Dispose()
    {
        ArchiveStream?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>Enumerates all entries.</summary>
    public IEnumerable<ResourceEntry> GetAllEntries() => _entries.Values;

    // -------------------------------------------------------------------------
    // Loading
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Opens and parses an archive from a file path using a raw Blowfish key string.
    /// </summary>
    /// <typeparam name="T">The concrete archive type to instantiate (e.g. <see cref="TTArchive2" />).</typeparam>
    /// <param name="filePath">Path to the archive file on disk.</param>
    /// <param name="blowFishKey">Raw Blowfish key string. Pass <see cref="string.Empty" /> for unencrypted archives.</param>
    /// <returns>A fully loaded archive instance.</returns>
    /// <exception cref="FileNotFoundException">Thrown if <paramref name="filePath" /> does not exist.</exception>
    /// <exception cref="InvalidDataException">Thrown if the archive header is corrupt or unrecognised.</exception>
    public static T Load<T>(string filePath, string blowFishKey) where T : Archive, new()
    {
        FileStream stream = File.OpenRead(filePath);
        return Load<T>(stream, blowFishKey);
    }

    /// <summary>
    ///     Opens and parses an archive from a stream using a raw Blowfish key string.
    /// </summary>
    /// <typeparam name="T">The concrete archive type to instantiate.</typeparam>
    /// <param name="stream">
    ///     A readable stream positioned at the start of the archive. Must remain open for the lifetime of the
    ///     returned archive.
    /// </param>
    /// <param name="blowFishKey">Raw Blowfish key string. Pass <see cref="string.Empty" /> for unencrypted archives.</param>
    /// <returns>A fully loaded archive instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidDataException">Thrown if the archive header is corrupt or unrecognised.</exception>
    public static T Load<T>(Stream stream, string blowFishKey) where T : Archive, new()
    {
        T archive = new();
        archive.ArchiveStream = stream ?? throw new ArgumentNullException(nameof(stream));
        archive.ArchiveStream.Position = 0;
        archive.Info.BlowfishKey = blowFishKey;
        archive.Activate();
        return archive;
    }

    /// <summary>
    ///     Opens and parses an archive from a file path using a well-known game Blowfish key.
    /// </summary>
    /// <typeparam name="T">The concrete archive type to instantiate.</typeparam>
    /// <param name="filePath">Path to the archive file on disk.</param>
    /// <param name="gameKey">The game key enum value whose Blowfish bytes will be used.</param>
    /// <returns>A fully loaded archive instance.</returns>
    public static T Load<T>(string filePath, T3BlowfishKey gameKey) where T : Archive, new()
        => Load<T>(filePath, gameKey.GetBlowfishKey());

    /// <summary>
    ///     Opens and parses an archive from a stream using a well-known game Blowfish key.
    /// </summary>
    /// <typeparam name="T">The concrete archive type to instantiate.</typeparam>
    /// <param name="stream">A readable stream positioned at the start of the archive.</param>
    /// <param name="gameKey">The game key enum value whose Blowfish bytes will be used.</param>
    /// <returns>A fully loaded archive instance.</returns>
    public static T Load<T>(Stream stream, T3BlowfishKey gameKey) where T : Archive, new()
        => Load<T>(stream, gameKey.GetBlowfishKey());

    /// <summary>
    ///     Parses the archive header from <see cref="ArchiveStream" /> and populates the entry table.
    ///     Called exactly once, immediately after the stream and <see cref="Info" /> are initialised.
    /// </summary>
    protected abstract void Activate();

    // -------------------------------------------------------------------------
    // Entry lookup
    // -------------------------------------------------------------------------

    /// <summary>Returns <see langword="true" /> if an entry with the given CRC-64 hash exists.</summary>
    public bool HasResource(ulong crc64) => _entries.ContainsKey(crc64);

    /// <summary>Returns <see langword="true" /> if an entry with the given filename exists (CRC-64 computed internally).</summary>
    public bool HasResource(string name) => HasResource(Crc64.Compute(name));

    /// <summary>Returns <see langword="true" /> if an entry matching the given <see cref="Symbol" /> exists.</summary>
    public bool HasResource(Symbol symbol) => HasResource(symbol.Crc64);

    /// <summary>Finds an entry by CRC-64 hash, or returns <see langword="null" /> if not found.</summary>
    public ResourceEntry? FindResource(ulong crc64) => _entries.GetValueOrDefault(crc64);

    /// <summary>Finds an entry by filename (CRC-64 computed internally), or returns <see langword="null" /> if not found.</summary>
    public ResourceEntry? FindResource(string name) => FindResource(Crc64.Compute(name));

    /// <summary>Finds an entry by <see cref="Symbol" />, or returns <see langword="null" /> if not found.</summary>
    public ResourceEntry? FindResource(Symbol symbol) => FindResource(symbol.Crc64);

    /// <summary>
    ///     Returns all entries whose filenames match a simple glob pattern (e.g. <c>"*.d3dtx"</c>).
    /// </summary>
    /// <param name="pattern">
    ///     A glob expression passed to <see cref="FileSystemName.MatchesSimpleExpression" />.
    ///     Supports <c>*</c> (any characters) and <c>?</c> (single character) wildcards.
    /// </param>
    public IEnumerable<ResourceEntry> FindResourcesByMask(string pattern)
        => GetAllEntries().Where(e => FileSystemName.MatchesSimpleExpression(pattern, e.Name));

    /// <summary>
    ///     Returns the filenames of all entries matching a simple glob pattern.
    /// </summary>
    /// <param name="pattern">A glob expression (see <see cref="FindResourcesByMask" />).</param>
    public IEnumerable<string> FindNamesByMask(string pattern)
        => FindResourcesByMask(pattern).Select(e => e.Name);

    // -------------------------------------------------------------------------
    // Entry table management (used by subclasses)
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Replaces the entire entry map with the given collection.
    ///     Must be called by subclasses at the end of <see cref="Activate" /> after all entries are parsed.
    /// </summary>
    /// <param name="entries">The parsed entries to register. Duplicate CRC-64 keys overwrite earlier values.</param>
    protected void SetEntries(IEnumerable<ResourceEntry> entries)
    {
        _entries.Clear();
        foreach (ResourceEntry e in entries)
        {
            _entries[e.NameCrc] = e;
        }
    }

    // -------------------------------------------------------------------------
    // Extraction
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Opens a stream over a resource's data by filename.
    ///     Returns <see langword="null" /> if no entry with that name exists.
    /// </summary>
    public Stream? OpenResource(string name) => OpenResource(FindResource(name));

    /// <summary>
    ///     Opens a stream over a resource's data by CRC-64 hash.
    ///     Returns <see langword="null" /> if no matching entry exists.
    /// </summary>
    public Stream? OpenResource(ulong crc64) => OpenResource(FindResource(crc64));

    /// <summary>
    ///     Opens a stream over a resource's data by <see cref="Symbol" />.
    ///     Returns <see langword="null" /> if no matching entry exists.
    /// </summary>
    public Stream? OpenResource(Symbol symbol) => OpenResource(FindResource(symbol));

    /// <summary>Extracts a file from the archive given its entry record.</summary>
    protected abstract Stream? OpenResource(ResourceEntry? entry);

    /// <summary>
    ///     Extracts all entries to <paramref name="destinationPath" />, preserving any directory
    ///     structure encoded in entry names.  Entries are extracted in ascending offset order to
    ///     minimise seek distance on the underlying stream.
    /// </summary>
    /// <param name="destinationPath">
    ///     Root folder to write into. Created (including parents) if it does not already exist.
    /// </param>
    public virtual void ExtractAll(string destinationPath)
    {
        Directory.CreateDirectory(destinationPath);

        foreach (ResourceEntry entry in _entries.Values.OrderBy(e => e.Offset))
        {
            string fullPath = Path.Combine(destinationPath, entry.Name);
            string? dir = Path.GetDirectoryName(fullPath);
            if (dir is not null)
            {
                Directory.CreateDirectory(dir);
            }

            using Stream? data = OpenResource(entry.NameCrc);
            if (data is null)
            {
                Toolkit.Instance.Logger.LogWarning($"[Archive] Skipping '{entry.Name}' — extraction returned null.");
                continue;
            }

            using FileStream output = File.Create(fullPath);
            data.Position = 0;
            data.CopyTo(output);
        }
    }

    // -------------------------------------------------------------------------
    // Encryption / compression helpers
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Returns <see langword="true" /> if the archive uses standard or modified Blowfish encryption.
    /// </summary>
    protected bool IsEncrypted() => Info.Flags.IsEncrypted();


    /// <summary>
    ///     Returns <see langword="true" /> if the archive uses any form of chunk compression
    ///     (zlib, raw deflate, or Oodle).
    /// </summary>
    protected bool IsCompressed() => Info.Flags.IsCompressed();

    // -------------------------------------------------------------------------
    // Creation (static helpers)
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Creates a new archive from a sequence of named streams and writes it to <paramref name="output" />.
    /// </summary>
    /// <typeparam name="TArchive">
    ///     The archive type to write. Must be <see cref="TTArchive" /> or <see cref="TTArchive2" />.
    /// </typeparam>
    /// <param name="output">Writable stream to receive the archive bytes. Left open after the call.</param>
    /// <param name="entries">
    ///     Sequence of <c>(entry name, data stream)</c> pairs. Data streams are read from their current
    ///     position.
    /// </param>
    /// <param name="options">Controls compression, encryption, chunk size, and archive version.</param>
    /// <exception cref="NotSupportedException">Thrown if <typeparamref name="TArchive" /> is not a supported type.</exception>
    public static void Create<TArchive>(
        Stream output,
        IEnumerable<(string name, Stream dataStream)> entries,
        ArchiveWriteOptions options)
        where TArchive : Archive, new()
    {
        if (typeof(TArchive) == typeof(TTArchive))
        {
            TTArchive.Create(output, entries, options);
        }
        else if (typeof(TArchive) == typeof(TTArchive2))
        {
            TTArchive2.Create(output, entries, options);
        }
        else
        {
            throw new NotSupportedException($"Archive type {typeof(TArchive).Name} does not support creation.");
        }
    }

    /// <summary>
    ///     Creates a new archive from a sequence of named streams and writes it to a file at <paramref name="outputPath" />.
    /// </summary>
    /// <typeparam name="TArchive">The archive type to write.</typeparam>
    /// <param name="outputPath">Destination file path. Created or overwritten.</param>
    /// <param name="entries">Sequence of <c>(entry name, data stream)</c> pairs.</param>
    /// <param name="options">Controls compression, encryption, chunk size, and archive version.</param>
    public static void Create<TArchive>(
        string outputPath,
        IEnumerable<(string name, Stream dataStream)> entries,
        ArchiveWriteOptions options)
        where TArchive : Archive, new()
    {
        using FileStream fs = new(outputPath, FileMode.Create, FileAccess.Write);
        Create<TArchive>(fs, entries, options);
    }

    /// <summary>
    ///     Creates a new archive from all files in the top level of <paramref name="folderPath" />,
    ///     using each file's name (without directory) as the entry name.
    /// </summary>
    /// <typeparam name="TArchive">The archive type to write.</typeparam>
    /// <param name="folderPath">Folder to scan. Only the top-level directory is searched.</param>
    /// <param name="outputPath">Destination file path. Created or overwritten.</param>
    /// <param name="options">Controls compression, encryption, chunk size, and archive version.</param>
    /// <param name="searchPattern">Optional file glob (e.g. <c>"*.d3dtx"</c>). Defaults to <c>"*"</c>.</param>
    public static void CreateFromFolder<TArchive>(
        string folderPath,
        string outputPath,
        ArchiveWriteOptions options,
        string searchPattern = "*")
        where TArchive : Archive, new()
    {
        List<(string name, Stream stream)> entries = new();
        try
        {
            foreach (string filePath in Directory.GetFiles(folderPath, searchPattern, SearchOption.TopDirectoryOnly))
            {
                entries.Add((Path.GetFileName(filePath), File.OpenRead(filePath)));
            }

            Create<TArchive>(outputPath, entries, options);
        }
        finally
        {
            foreach ((string _, Stream stream) in entries)
            {
                stream.Dispose();
            }
        }
    }

    /// <summary>
    ///     Creates a new archive from an explicit list of file paths, using each file's name
    ///     (without directory) as the entry name.
    /// </summary>
    /// <typeparam name="TArchive">The archive type to write.</typeparam>
    /// <param name="outputPath">Destination file path. Created or overwritten.</param>
    /// <param name="filePaths">Full paths of the files to include.</param>
    /// <param name="options">Controls compression, encryption, chunk size, and archive version.</param>
    public static void CreateFromFiles<TArchive>(
        string outputPath,
        IEnumerable<string> filePaths,
        ArchiveWriteOptions options)
        where TArchive : Archive, new()
    {
        List<(string name, Stream stream)> entries = new();
        try
        {
            foreach (string filePath in filePaths)
            {
                entries.Add((Path.GetFileName(filePath), File.OpenRead(filePath)));
            }

            Create<TArchive>(outputPath, entries, options);
        }
        finally
        {
            foreach ((string _, Stream stream) in entries)
            {
                stream.Dispose();
            }
        }
    }
}

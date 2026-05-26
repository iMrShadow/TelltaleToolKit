using System.Text;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives.IO;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit.TelltaleArchives.Formats;

/// <summary>
///     Reader for the <c>.ttarch2</c> archive format (TTA2 / TTA3 / TTA4).
/// </summary>
/// <remarks>
///     <para>
///         A <c>.ttarch2</c> file is double-layered: an outer <em>container</em> header
///         (one of the <c>TTC*</c> four-byte magic values) wraps the actual TTA2/3/4 archive.
///         The container controls encryption and compression at the chunk level; the inner header
///         describes the file-entry table. This split is now modelled explicitly by
///         <see cref="ContainerStream" />, which the read path opens before parsing the TTA header.
///     </para>
///     <para>
///         Entry lookup is O(1) via the base-class <see cref="Archive.Entries" /> dictionary.
///     </para>
/// </remarks>
public class TTArchive2 : Archive
{
    private const int NamePageSize = 0x10000;
    private ContainerStream? _containerStream;

    protected override void Activate()
    {
        // Open a ContainerStream over the raw file stream.
        // ContainerStream reads the TTCN/TTCE/TTCZ/etc. header, sets up chunk tables,
        // and then exposes a seekable view of the decrypted+decompressed payload.
        _containerStream = new ContainerStream(ArchiveStream!, Info.BlowfishKey);
        // Copy container state back to Info so the rest of the code (and callers
        // that inspect Info) see the right values.
        Info.ContainerMagic = _containerStream.ContainerMagic;
        Info.Flags = _containerStream.Flags;
        Info.ChunkSize = _containerStream.ChunkSize;
        Info.ChunkCount = _containerStream.ChunkCount;
        Info.ChunkBlockSizes = _containerStream.ChunkBlockSizes;

        ReadTtaHeader(_containerStream);
    }

    private void ReadTtaHeader(ContainerStream container)
    {
        using BinaryReader reader = new(container, Encoding.UTF8, true);

        Info.Version = (TTArchiveVersion)reader.ReadUInt32();

        if (Info.Version is TTArchiveVersion.Tta3 or TTArchiveVersion.Tta2)
        {
            _ = reader.ReadUInt32(); // unknown / reserved
        }

        // mNamePageCount — number of 64 KiB pages in the name stream (mpNameStream).
        uint fileNameBufferSize = reader.ReadUInt32();
        if (fileNameBufferSize > 0x10000000)
        {
            throw new InvalidDataException($"[TTArchive2] Name buffer size {fileNameBufferSize} is implausibly large.");
        }

        int fileCount = reader.ReadInt32();
        if (fileCount > 0xFFFFF)
        {
            throw new InvalidDataException($"[TTArchive2] File count {fileCount} is implausibly large.");
        }

        // FilesOffset: position inside the decoded payload where resource data begins —
        // immediately after the entry table and the name stream.

        List<ResourceEntry> entries = new(fileCount);

        for (int i = 0; i < fileCount; i++)
        {
            ulong crc64 = reader.ReadUInt64(); // mNameCRC
            ulong fileOffset = reader.ReadUInt64(); // mOffset

            if (Info.Version is TTArchiveVersion.Tta2)
            {
                _ = reader.ReadUInt32(); // extra field present only in TTA2. It's an alternative to namepages.
            }

            uint size = reader.ReadUInt32(); // mSize
            uint preloadSize = reader.ReadUInt32(); // mPreloadSize

            ushort namePageIndex = 0;
            ushort namePageOff = 0;

            if (Info.Version is not TTArchiveVersion.Tta2)
            {
                namePageIndex = reader.ReadUInt16(); // mNamePageIndex
                namePageOff = reader.ReadUInt16(); // mNamePageOffset
            }

            entries.Add(new ResourceEntry
            {
                NameCrc = crc64,
                Offset = fileOffset,
                Size = size,
                PreloadSize = preloadSize,
                NamePageIndex = namePageIndex,
                NamePageOffset = namePageOff
            });
        }

        Info.FilesOffset = (ulong)(reader.BaseStream.Position + fileNameBufferSize);

        // ---- Name stream ----
        // Consists of namePageCount contiguous 64 KiB pages.
        // Each entry addresses its name as: absoluteByte = pageIndex * 0x10000 + pageOffset.
        ResolveNames(reader, entries, fileNameBufferSize);

        SetEntries(entries);
    }

    private static void ResolveNames(
        BinaryReader reader,
        List<ResourceEntry> entries,
        long nameStreamLen)
    {
        if (nameStreamLen == 0 || entries.Count == 0)
        {
            return;
        }

        long namesStart = reader.BaseStream.Position;

        List<(ResourceEntry Entry, long Position)> sorted = entries
            .Select(e => (Entry: e, Position: (long)e.NamePageIndex * NamePageSize + e.NamePageOffset))
            .OrderBy(x => x.Position)
            .ToList();

        reader.BaseStream.Seek(namesStart + sorted[0].Position, SeekOrigin.Begin);

        const int maxNameLength = 256; // sane limit to prevent infinite loops

        foreach ((ResourceEntry entry, long targetPos) in sorted)
        {
            long absolutePos = namesStart + targetPos;
            if (reader.BaseStream.Position != absolutePos)
            {
                reader.BaseStream.Seek(absolutePos, SeekOrigin.Begin);
            }

            int namePos = entry.NamePageIndex * NamePageSize + entry.NamePageOffset;

            if ((uint)namePos >= (uint)nameStreamLen)
            {
                Toolkit.Instance.Logger.LogWarning(
                    $"[TTArchive2] 0x{entry.NameCrc:X16}: name position 0x{namePos:X} " +
                    $"exceeds name stream length 0x{nameStreamLen:X}. Name left null.");
                continue;
            }

            StringBuilder nameBuilder = new();
            for (int i = 0; i < maxNameLength; i++)
            {
                byte b = reader.ReadByte();
                if (b == 0)
                {
                    break;
                }

                nameBuilder.Append((char)b);
            }

            entry.Name = nameBuilder.ToString();
        }
    }

    protected override Stream? OpenResource(ResourceEntry? entry)
    {
        if (_containerStream == null)
        {
            throw new InvalidOperationException("Archive not properly loaded.");
        }

        if (entry is null)
        {
            return null;
        }

        return new SubStream(_containerStream, (long)(Info.FilesOffset + entry.Offset), entry.Size);
    }

    public override void Dispose()
    {
        _containerStream?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Creates a new .ttarch2 archive from a collection of named streams.
    /// </summary>
    /// <param name="output">Stream to write the final archive to (will be left open).</param>
    /// <param name="entries">Sequence of (entry name, data stream) pairs.</param>
    /// <param name="options">Container options (compression, encryption, chunk size, Blowfish key).</param>
    public static void Create(Stream output, IEnumerable<string> entries, ArchiveWriteOptions options)
    {
        List<(string path, Stream dataStream)> finalEntries = [];

        foreach (string path in entries)
        {
            try
            {
                FileStream fs = File.Open(path, FileMode.Open);
                finalEntries.Add((path, fs));
            }
            catch (Exception e)
            {
                Toolkit.Instance.Logger.LogError($"[TTArchive2] Failed to open file {path}: {e.Message}");
            }
        }

        Create(output, finalEntries, options);
    }

    /// <summary>
    ///     Creates a new .ttarch2 archive from a collection of named streams.
    /// </summary>
    /// <param name="output">Stream to write the final archive to (will be left open).</param>
    /// <param name="entries">Sequence of (entry name, data stream) pairs.</param>
    /// <param name="options">Container options (compression, encryption, chunk size, Blowfish key).</param>
    public static void Create(Stream output, IEnumerable<(string name, Stream dataStream)> entries,
        ArchiveWriteOptions options)
    {
        // Collect metadata (no data buffering)
        List<(string name, Stream stream, long length, ulong crc64)> entryList = new();
        foreach ((string name, Stream stream) in entries)
        {
            long len = stream.Length;
            entryList.Add((name, stream, len, Crc64.Compute(name)));
        }

        // Build name-page stream and positions
        using MemoryStream nameMs = new();
        (ushort pageIndex, ushort pageOffset)[] namePositions =
            new (ushort pageIndex, ushort pageOffset)[entryList.Count];
        for (int i = 0; i < entryList.Count; i++)
        {
            string name = entryList[i].name;
            long pos = nameMs.Position;
            namePositions[i] = ((ushort)(pos / 0x10000), (ushort)(pos % 0x10000));
            byte[] nameBytes = Encoding.ASCII.GetBytes(name);
            nameMs.Write(nameBytes);
            nameMs.WriteByte(0);
        }

        uint namePageCount = (uint)((nameMs.Length + 0x10000 - 1) / 0x10000);
        long paddedLen = namePageCount * 0x10000;
        nameMs.SetLength(paddedLen);
        byte[] namePageBytes = nameMs.ToArray();

        // Calculate file offsets (cumulative sum of sizes)
        ulong[] fileOffsets = new ulong[entryList.Count];
        ulong currentOffset = 0;
        for (int i = 0; i < entryList.Count; i++)
        {
            fileOffsets[i] = currentOffset;
            currentOffset += (ulong)entryList[i].length;
        }

        // Write inner payload to a temporary file
        string tempFile = Path.GetTempFileName();
        try
        {
            using (FileStream inner = new(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096,
                       FileOptions.DeleteOnClose | FileOptions.SequentialScan))
            using (BinaryWriter bw = new(inner, Encoding.UTF8, true))
            {
                // Header
                bw.Write((uint)options.TTArchiveVersion);
                if (options.TTArchiveVersion is TTArchiveVersion.Tta2 or TTArchiveVersion.Tta3)
                {
                    bw.Write(0u);
                }

                bw.Write(namePageCount);
                bw.Write((uint)entryList.Count);

                // Entry table
                for (int i = 0; i < entryList.Count; i++)
                {
                    bw.Write(entryList[i].crc64);
                    bw.Write(fileOffsets[i]);
                    if (options.TTArchiveVersion is TTArchiveVersion.Tta2)
                    {
                        bw.Write(0u);
                    }

                    bw.Write((uint)entryList[i].length);
                    bw.Write(0u);
                    bw.Write(namePositions[i].pageIndex);
                    bw.Write(namePositions[i].pageOffset);
                }

                // Name pages
                bw.Write(namePageBytes);

                // File data
                for (int i = 0; i < entryList.Count; i++)
                {
                    Stream stream = entryList[i].stream;
                    stream.Position = 0;
                    stream.CopyTo(inner);
                }
            }

            // Now wrap with container
            using (FileStream innerReader = new(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                       FileOptions.DeleteOnClose | FileOptions.SequentialScan))
            {
                ContainerStream.Create(output, innerReader, options);
            }
        }
        finally
        {
            // Ensure temp file is deleted (FileOptions.DeleteOnClose should handle, but just in case)
            try
            {
                File.Delete(tempFile);
            }
            catch
            {
                // ignored
            }
        }
    }
}

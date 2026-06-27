using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Hashing;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using TelltaleToolKit.Encryption;
using TelltaleToolKit.IO.Streams;
using Crc64 = TelltaleToolKit.Hashing.Crc64;

namespace TelltaleToolKit.IO.Archives.Formats;

/// <summary>
///     Reader for the original <c>.ttarch</c> archive format (versions 0–9).
/// </summary>
/// <remarks>
///     All <c>.ttarch</c> implementations are based on ttarchext and TTG Tools research.
/// </remarks>
public class TTArchive : Archive
{
    private Blowfish _blowfish;
    // -------------------------------------------------------------------------
    // Legacy stream (version 0 / pre-versioned encrypted archives)
    // -------------------------------------------------------------------------

    private Stream? _dataStream; // decompressed view of the entire file-data region

    protected override void Activate()
    {
        using BinaryReader reader = new(BaseStream!, Encoding.UTF8, true);

        Info.Version = (TTArchiveVersion)reader.ReadUInt32();
        int version = (int)Info.Version;

        // Anything outside 1–9 falls back to the legacy reader
        // (version 0, or a number so large it wrapped - both indicate the old format).
        // The only issue with this is the folder number - if the folders are between 1 and 9, this fails.
        if (version is < 1 or > 9)
        {
            ReadLegacyStream(reader);
            return;
        }

        _blowfish = new Blowfish(Info.BlowfishKey, (int)Info.Version);

        // ---- Version 1+ header ----
        uint decryptionMode = reader.ReadUInt32();
        if (decryptionMode > 1)
        {
            Toolkit.Instance.Logger.LogWarning(
                $"Decryption mode {decryptionMode} is not supported. Falling back to encrypted.");
        }

        Info.Flags |= decryptionMode == 1 ? ArchiveFlags.IsEncrypted : ArchiveFlags.None;

        if (version >= 2)
        {
            _ = reader.ReadUInt32(); // unknown field
        }

        // files_mode: 2 = compressed, 1 - normal
        uint filesMode = 1;

        // ---- Compression (version 3+) ----
        if (version >= 3)
        {
            filesMode = reader.ReadUInt32();

            if (filesMode > 2)
            {
                Toolkit.Instance.Logger.LogWarning(
                    $"Files mode {filesMode} is not supported. Falling back to uncompressed.");
            }

            Info.ChunkCount = reader.ReadUInt32();

            Info.ChunkBlockSizes = new ulong[Info.ChunkCount];

            for (int i = 0; i < Info.ChunkCount; i++)
            {
                Info.ChunkBlockSizes[i] = reader.ReadUInt32();
            }

            if (Info.ChunkCount > 0)
            {
                // Both flags are set; Decompress sorts out which algorithm is active.
                Info.ChunkSize = 0x10000;
            }

            _ = reader.ReadUInt32(); // total file-data region size AFTER the ttarch header is read (and the name)
        }

        if (version >= 4)
        {
            _ = reader.ReadUInt32(); // unknown (possibly priority)
            _ = reader.ReadUInt32(); // unknown
        }

        if (version >= 5)
        {
            uint xMode1 = reader.ReadUInt32(); // unknown
            uint xMode2 = reader.ReadUInt32(); // unknown
            Info.Flags |= xMode1 == 1 || xMode2 == 1 ? ArchiveFlags.IsXMode : ArchiveFlags.None;
        }

        if (version >= 7)
        {
            const uint blockSizeFactor = 1024;
            Info.ChunkSize = reader.ReadUInt32() * blockSizeFactor;
        }

        if (version >= 8)
        {
            // This is a boolean value that indicates whether a symbol table should be created or not.
            byte createSymbolTable = reader.ReadByte();

            if (createSymbolTable > 0)
            {
                _ = reader.ReadInt32(); // no idea if this is true
            }
        }

        if (version >= 9)
        {
            _ = reader.ReadUInt32(); // crc32 for checksum of the header (decompressed and decrypted)
        }

        int infoHeaderSize = reader.ReadInt32();

        Compression.Mode compression = Compression.Mode.None;

        // This is compressed
        byte[] header;
        if (version >= 6 && filesMode == 2)
        {
            int compressedHeaderSize = reader.ReadInt32();
            header = reader.ReadBytes(compressedHeaderSize);

            compression = Compression.DetectMode(header);

            Info.Flags |= compression is Compression.Mode.Deflate
                ? ArchiveFlags.IsRawDeflateCompressed
                : ArchiveFlags.IsZlibCompressed;

            header = Compression.Decompress(header, compression);
        }
        else
        {
            header = reader.ReadBytes(infoHeaderSize);
        }

        if (decryptionMode == 1)
        {
            _blowfish.Decipher(header, header.Length);
        }

        Info.FilesOffset = (ulong)reader.BaseStream.Position;
        ParseEntries(new MemoryStream(header));

        _dataStream = BuildFileDataStream(version, filesMode, decryptionMode, compression);

        if (compression is Compression.Mode.None && _dataStream is TtarchiveChunkedDataStream chunkedStream)
        {
            OpenResource(Entries.First().Key)?.ReadByte();

            Info.Flags |= chunkedStream.Compression is Compression.Mode.Deflate
                ? ArchiveFlags.IsRawDeflateCompressed
                : ArchiveFlags.IsZlibCompressed;
        }
    }

    /// <summary>
    ///     Reads the oldest encrypted format where the file begins with a raw header-size
    ///     field rather than a version number.
    /// </summary>
    /// <remarks>
    ///     The 128-byte threshold distinguishes a genuine header-size field
    ///     (always larger) from what would otherwise look like a small version number.
    ///     Archives below the threshold are treated as plain unencrypted legacy files.
    /// </remarks>
    private void ReadLegacyStream(BinaryReader reader)
    {
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        Info.Version = TTArchiveVersion.Legacy;
        _blowfish = new Blowfish(Info.BlowfishKey, (int)Info.Version);

        uint headerSize = reader.ReadUInt32();

        // This is a lame work-around, because Telltale had stopped reading the original archives.
        // There shouldn't be archives with more than 128 folders
        if (headerSize > 128)
        {
            // Encrypted legacy archive — header block is Blowfish-decrypted in place.
            Info.Flags |= ArchiveFlags.IsEncrypted;

            byte[] header = reader.ReadBytes((int)headerSize);
            _blowfish.Decipher(header, header.Length);

            ParseEntries(new MemoryStream(header));
            Info.FilesOffset = (ulong)reader.BaseStream.Position;
            _dataStream = BuildFileDataStream(0, 0, 1, Compression.Mode.None);
        }
        else
        {
            // Plain legacy archive — entries are written directly into the stream.
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            ParseEntries(reader.BaseStream);
            Info.FilesOffset = reader.ReadUInt32();
            _ = reader.ReadUInt32(); // file size
            _dataStream = BuildFileDataStream(0, 0, 0, Compression.Mode.None);
        }
    }

    private void ParseEntries(Stream stream)
    {
        using BinaryReader reader = new(stream, Encoding.ASCII, true);

        int directoriesCount = reader.ReadInt32();
        for (int i = 0; i < directoriesCount; i++)
        {
            int nameLength = reader.ReadInt32();

            if (nameLength is < 1 or > 1023)
            {
                throw new ArgumentOutOfRangeException(
                    $"Error: directory name length {nameLength} is too long.", nameof(nameLength));
            }

            _ = Encoding.ASCII.GetString(reader.ReadBytes(nameLength));
        }

        uint fileCount = reader.ReadUInt32();

        List<ResourceEntry> entries = new((int)fileCount);

        for (int i = 0; i < fileCount; i++)
        {
            int nameLength = reader.ReadInt32();
            if (nameLength is < 1 or > 255)
            {
                throw new ArgumentOutOfRangeException(
                    $"Error: File name length {nameLength} is too long.", nameof(nameLength));
            }

            string name = Encoding.ASCII.GetString(reader.ReadBytes(nameLength));
            _ = reader.ReadInt32(); // folder-index field — always 0, purpose unclear
            uint offset = reader.ReadUInt32();
            uint size = reader.ReadUInt32();

            entries.Add(new ResourceEntry { NameCrc = Crc64.Compute(name), Name = name, Offset = offset, Size = size });
        }

        SetEntries(entries);
    }

    private Stream BuildFileDataStream(int version, uint filesMode, uint decryptionMode,
        Compression.Mode compressionMode)
    {
        // No compression → raw substream (may still need per-file decryption later)
        if (Info.ChunkCount == 0 || filesMode != 2)
        {
            return new SubStream(BaseStream!, (long)Info.FilesOffset, long.MaxValue);
        }

        // Compressed chunked region → on-demand decompression
        return new TtarchiveChunkedDataStream(
            BaseStream!,
            (long)Info.FilesOffset,
            Info.ChunkSize,
            Info.ChunkBlockSizes,
            compressionMode,
            Info.BlowfishKey,
            version,
            decryptionMode == 1);
    }

    protected override Stream? OpenResource(ResourceEntry? entry)
    {
        if (_dataStream == null)
        {
            throw new InvalidOperationException("Archive not loaded.");
        }

        return entry is null ? null : new SubStream(_dataStream, (long)entry.Offset, entry.Size);
    }

    /// <summary>
    ///     Creates a new .ttarch archive (versions 1–9) directly to a stream.
    /// </summary>
    /// <param name="output">Stream to write the archive to (left open).</param>
    /// <param name="entries">Sequence of (filename, data stream) pairs. Filenames are ASCII, paths are flattened.</param>
    /// <param name="options">Archive options (version, encryption, compression, chunk size, Blowfish key).</param>
    public static void Create(Stream output, IEnumerable<(string name, Stream dataStream)> entries,
        ArchiveWriteOptions options)
    {
        if (!output.CanSeek)
        {
            throw new ArgumentException("Output stream must be seekable for patching header fields.", nameof(output));
        }

        int version = options.TTArchiveVersion.ToJsonNumber();

        switch (version)
        {
            case < 0 or > 9:
                throw new NotSupportedException($"TTArchive.Create supports versions 1-9, got {version}.");
            case 0:
                CreateLegacy(output, entries, options);
                return;
        }

        bool encrypt = options.Encrypt;
        bool compressFileData = options.Compression != Compression.Mode.None && version >= 3;
        bool compressHeader = compressFileData && version >= 6; // compressed entry table block (filesMode=2)

        Blowfish bf = new(options.BlowfishKey, version);

        // ------------------------------------------------------------------------
        // 1. Collect metadata & compute file offsets (within file-data region)
        // ------------------------------------------------------------------------
        List<(string name, Stream stream, long length, ulong crc64, uint offset)> entryList = new();
        uint currentOffset = 0;
        foreach ((string name, Stream stream) in entries)
        {
            long len = stream.Length;
            if (len > uint.MaxValue)
            {
                throw new InvalidOperationException($"File {name} exceeds 4 GiB – not supported in TTArchive.");
            }

            entryList.Add((name, stream, len, Crc64.Compute(name), currentOffset));
            currentOffset += (uint)len;
        }

        uint totalFileDataSize = currentOffset;

        // ------------------------------------------------------------------------
        // 2. Build entry table header (directories + files)
        // ------------------------------------------------------------------------
        using MemoryStream headerMs = new();
        using BinaryWriter headerWriter = new(headerMs, Encoding.ASCII, true);

        // directories (none)
        headerWriter.Write(0); // directoriesCount
        // files
        headerWriter.Write(entryList.Count);
        foreach ((string name, _, long length, ulong _, uint offset) in entryList)
        {
            byte[] nameBytes = Encoding.ASCII.GetBytes(name);
            if (nameBytes.Length > 255)
            {
                throw new ArgumentException($"Filename too long: {name}");
            }

            headerWriter.Write(nameBytes.Length);
            headerWriter.Write(nameBytes);
            headerWriter.Write(0); // folder-index (always 0)
            headerWriter.Write(offset);
            headerWriter.Write((uint)length);
        }

        byte[] rawHeader = headerMs.ToArray();
        int rawHeaderSize = rawHeader.Length;

        // ------------------------------------------------------------------------
        // 3. Helper to compress a block (raw deflate or zlib)
        // ------------------------------------------------------------------------
        byte[] CompressBlock(byte[] data)
        {
            using MemoryStream ms = new();
            if (options.Compression == Compression.Mode.Zlib)
            {
                using DeflaterOutputStream zlib = new(ms);
                zlib.Write(data, 0, data.Length);
                zlib.Finish();
            }
            else
            {
                using DeflateStream deflate = new(ms, CompressionLevel.Optimal, true);
                deflate.Write(data, 0, data.Length);
            }

            return ms.ToArray();
        }

        // ------------------------------------------------------------------------
        // 4. Write outer header (with placeholders) and entry table
        // ------------------------------------------------------------------------
        MemoryStream headerBuffer = new(); // buffer for header part (we'll write it after patching)
        using BinaryWriter headerBufWriter = new(headerBuffer, Encoding.UTF8, true);

        headerBufWriter.Write((uint)version);
        headerBufWriter.Write(encrypt ? 1u : 0u); // decryptionMode
        if (version >= 2)
        {
            headerBufWriter.Write(0u); // unknown. Platform?
        }

        long chunkCountPos = -1, chunkTableStartPos = -1;
        if (version >= 3)
        {
            uint filesMode = compressFileData ? 2u : 1u;
            headerBufWriter.Write(filesMode);

            // chunkCount placeholder
            chunkCountPos = headerBufWriter.BaseStream.Position;
            headerBufWriter.Write(0u); // will be overwritten

            if (compressFileData)
            {
                // Reserve space for chunk block sizes list (max possible chunks)
                int maxChunks = (int)((totalFileDataSize + options.ChunkSize - 1) / options.ChunkSize);
                chunkTableStartPos = headerBufWriter.BaseStream.Position;
                for (int i = 0; i < maxChunks; i++)
                {
                    headerBufWriter.Write(0u); // placeholder for each compressed size
                }
            }
            else
            {
                // No compression: chunkCount = 0, then totalFileDataSize
                headerBufWriter.Write(totalFileDataSize);
            }
        }

        // Version 4 unknowns
        if (version >= 4)
        {
            headerBufWriter.Write(0u);
            headerBufWriter.Write(0u);
        }

        // Version 5 xMode
        if (version >= 5)
        {
            int xModeValue = options.XMode ? 1 : 0;
            headerBufWriter.Write(xModeValue); // xMode1
            headerBufWriter.Write(xModeValue); // xMode2
        }

        // Version 7 chunk size divisor
        if (version >= 7 && compressFileData)
        {
            headerBufWriter.Write(0x40);
        }

        // Version 8 symbol table flag
        if (version >= 8)
        {
            headerBufWriter.Write((byte)0); // createSymbolTable = false
        }

        // Version 9 CRC32 placeholder
        long crc32Pos = -1;
        if (version >= 9)
        {
            crc32Pos = headerBufWriter.BaseStream.Position;
            headerBufWriter.Write(0u);
        }

        // ---- Write entry table header (maybe compressed/encrypted) ----
        if (compressHeader)
        {
            byte[] compressedHeader = CompressBlock(rawHeader);
            if (encrypt)
            {
                bf.Encipher(compressedHeader, compressedHeader.Length);
            }

            headerBufWriter.Write(rawHeaderSize);
            headerBufWriter.Write(compressedHeader.Length);
            headerBufWriter.Write(compressedHeader);
        }
        else
        {
            if (encrypt)
            {
                bf.Encipher(rawHeader, rawHeader.Length);
            }

            headerBufWriter.Write(rawHeader.Length);
            headerBufWriter.Write(rawHeader);
        }

        // ------------------------------------------------------------------------
        // 5. Write file-data region (raw or compressed chunks)
        // ------------------------------------------------------------------------
        List<uint> compressedSizes = new();
        if (compressFileData)
        {
            int chunkSize = (int)options.ChunkSize;
            byte[] chunkBuffer = new byte[chunkSize];
            int bufferPos = 0;

            // We'll write the compressed chunks directly to output after the header buffer.
            // First, write the header buffer to output (to reserve space for header).
            headerBuffer.Position = 0;
            headerBuffer.CopyTo(output);

            // Process each file stream sequentially, filling chunkBuffer.
            foreach ((string _, Stream stream, long length, ulong _, uint _) in entryList)
            {
                stream.Position = 0;
                long bytesLeft = length;
                while (bytesLeft > 0)
                {
                    int want = chunkSize - bufferPos;
                    int readSize = (int)Math.Min(want, bytesLeft);
                    byte[] temp = new byte[readSize];
                    if (stream.Read(temp, 0, readSize) != readSize)
                    {
                        throw new EndOfStreamException();
                    }

                    Buffer.BlockCopy(temp, 0, chunkBuffer, bufferPos, readSize);
                    bufferPos += readSize;
                    bytesLeft -= readSize;

                    if (bufferPos != chunkSize)
                    {
                        continue;
                    }

                    // Full chunk: compress and write
                    byte[] compressed = CompressBlock(chunkBuffer);
                    if (encrypt)
                    {
                        bf.Encipher(compressed, compressed.Length);
                    }

                    output.Write(compressed, 0, compressed.Length);
                    compressedSizes.Add((uint)compressed.Length);
                    bufferPos = 0;
                }
            }

            // Final partial chunk
            if (bufferPos > 0)
            {
                byte[] lastChunk = new byte[bufferPos];
                Buffer.BlockCopy(chunkBuffer, 0, lastChunk, 0, bufferPos);
                byte[] compressed = CompressBlock(lastChunk);
                if (encrypt)
                {
                    bf.Encipher(compressed, compressed.Length);
                }

                output.Write(compressed, 0, compressed.Length);
                compressedSizes.Add((uint)compressed.Length);
            }

            // Now patch the header with actual chunk count and block sizes
            output.Seek(chunkCountPos, SeekOrigin.Begin);
            BinaryWriter bw = new(output, Encoding.UTF8, true);
            bw.Write((uint)compressedSizes.Count);
            // Write block sizes
            output.Seek(chunkTableStartPos, SeekOrigin.Begin);
            foreach (uint size in compressedSizes)
            {
                bw.Write(size);
            }

            // Seek back to end (optional)
            output.Seek(0, SeekOrigin.End);
        }
        else
        {
            // Uncompressed: write header buffer, then raw file data
            headerBuffer.Position = 0;
            headerBuffer.CopyTo(output);
            foreach ((string _, Stream stream, long _, ulong _, uint _) in entryList)
            {
                stream.Position = 0;
                stream.CopyTo(output);
            }
        }

        // ------------------------------------------------------------------------
        // 6. Patch CRC32 for version 9 (if applicable)
        // ------------------------------------------------------------------------
        if (version >= 9)
        {
            // CRC32 of decompressed+decrypted header (i.e., rawHeader)
            uint crc = ComputeCrc32(rawHeader);
            output.Seek(crc32Pos, SeekOrigin.Begin);
            BinaryWriter bw = new(output, Encoding.UTF8, true);
            bw.Write(crc);
            // Seek back to end
            output.Seek(0, SeekOrigin.End);
        }
    }

    private static uint ComputeCrc32(byte[] data)
    {
        Crc32 crc32 = new();
        crc32.Append(data);
        return crc32.GetCurrentHashAsUInt32();
    }

    /// <summary>
    ///     Creates a legacy (version 0) .ttarch archive, optionally encrypted.
    /// </summary>
    /// <param name="output">Stream to write the archive to (must be seekable for patching header size).</param>
    /// <param name="entries">Sequence of (filename, data stream) pairs. Filenames are ASCII.</param>
    /// <param name="options">
    ///     Archive options. <see cref="ArchiveWriteOptions.TTArchiveVersion" /> must be <see cref="TTArchiveVersion.Legacy" />
    ///     .
    ///     <see cref="ArchiveWriteOptions.Encrypt" /> determines whether the header block is encrypted.
    ///     <see cref="ArchiveWriteOptions.BlowfishKey" /> is required when Encrypt is true.
    /// </param>
    public static void CreateLegacy(Stream output, IEnumerable<(string name, Stream dataStream)> entries,
        ArchiveWriteOptions options)
    {
        if (options.TTArchiveVersion != TTArchiveVersion.Legacy)
        {
            throw new ArgumentException("Legacy creation requires TTArchiveVersion.Legacy", nameof(options));
        }

        if (options.Encrypt && string.IsNullOrEmpty(options.BlowfishKey))
        {
            throw new ArgumentException("Encryption requires a Blowfish key", nameof(options));
        }

        // ------------------------------------------------------------------------
        // 1. Collect metadata & compute file offsets
        // ------------------------------------------------------------------------
        List<(string name, Stream stream, long length, ulong crc64, uint offset)> entryList = new();
        uint currentOffset = 0;
        foreach ((string name, Stream stream) in entries)
        {
            long len = stream.Length;
            if (len > uint.MaxValue)
            {
                throw new InvalidOperationException($"File {name} exceeds 4 GiB – not supported in legacy TTArchive.");
            }

            entryList.Add((name, stream, len, Crc64.Compute(name), currentOffset));
            currentOffset += (uint)len;
        }

        uint totalFileDataSize = currentOffset;

        // ------------------------------------------------------------------------
        // 2. Build entry table (directories + files)
        // ------------------------------------------------------------------------
        using MemoryStream headerMs = new();
        using BinaryWriter headerWriter = new(headerMs, Encoding.ASCII, true);
        // directories (none)
        headerWriter.Write(0);
        // files
        headerWriter.Write(entryList.Count);
        foreach ((string name, _, long length, ulong _, uint offset) in entryList)
        {
            byte[] nameBytes = Encoding.ASCII.GetBytes(name);
            if (nameBytes.Length > 255)
            {
                throw new ArgumentException($"Filename too long: {name}");
            }

            headerWriter.Write(nameBytes.Length);
            headerWriter.Write(nameBytes);
            headerWriter.Write(0); // folder-index (always 0)
            headerWriter.Write(offset);
            headerWriter.Write((uint)length);
        }

        byte[] entryTable = headerMs.ToArray();

        // ------------------------------------------------------------------------
        // 3. Write the archive
        // ------------------------------------------------------------------------
        if (!options.Encrypt)
        {
            // Plain legacy: write entry table directly
            output.Write(entryTable, 0, entryTable.Length);
            // Then two trailing uints: filesOffset (where file data starts) and filesSize (total size of file data)
            long filesOffset = output.Position;
            using BinaryWriter bw = new(output, Encoding.UTF8, true);
            bw.Write((uint)filesOffset);
            bw.Write(totalFileDataSize);
        }
        else
        {
            // Encrypted legacy: write headerSize (encrypted block length) then encrypted entry table
            Blowfish blowfish = new(options.BlowfishKey, 0);
            byte[] encrypted = (byte[])entryTable.Clone();
            blowfish.Encipher(encrypted, encrypted.Length);
            uint headerSize = (uint)encrypted.Length;
            using BinaryWriter bw = new(output, Encoding.UTF8, true);
            bw.Write(headerSize);
            bw.Write(encrypted);
            // Files offset is right after the encrypted header
            // No trailing uints in encrypted legacy format according to reader (the reader sets FilesOffset to current position)
            // So we don't write anything else.
        }

        // ------------------------------------------------------------------------
        // 4. Write file data (raw, unencrypted)
        // ------------------------------------------------------------------------
        foreach ((string _, Stream stream, long _, ulong _, uint _) in entryList)
        {
            stream.Position = 0;
            stream.CopyTo(output);
        }
    }
}

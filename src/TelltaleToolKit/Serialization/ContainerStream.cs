using System.Text;
using TelltaleToolKit.TelltaleArchives;
using TelltaleToolKit.TelltaleArchives.Caching;
using TelltaleToolKit.TelltaleArchives.Formats;
using TelltaleToolKit.TelltaleArchives.IO;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.Serialization;

/// <summary>
///     A forward-readable, seekable <see cref="Stream" /> that decodes the outer
///     <em>Telltale Tool Container</em> layer of a <c>.ttarch2</c> file.
/// </summary>
/// <remarks>
///     <para>
///         <strong>Single responsibility:</strong> parse the container header and expose the
///         inner TTA2/3/4 payload as a readable stream so <see cref="TTArchive2" /> can walk
///         the entry table and name-page stream. It is disposed immediately after that forward
///         pass — extraction never goes through it.
///     </para>
///     <para>
///         <strong>No caching.</strong> Header parsing is a single sequential forward pass over
///         a tiny fraction of the archive. Decoding a chunk once and discarding it is always
///         enough. This avoids any heap accumulation for 2 GB+ archives.
///     </para>
///     <para>
///         <strong>O(1) chunk offset lookup.</strong> A prefix-sum table (<see cref="_chunkPrefixSums" />)
///         is built once from the chunk-size array during construction, so seeking to any chunk
///         does not require walking the size array.
///     </para>
/// </remarks>
public sealed class ContainerStream : Stream
{
    private readonly string _blowfishKey;
    private readonly IChunkCache _cache;

    // Prefix-sum table: _chunkPrefixSums[i] = sum of compressed sizes of chunks 0..i-1.
    // Length = ChunkCount + 1 so _chunkPrefixSums[ChunkCount] = total compressed payload size.
    // Built once during construction for O(1) chunk-offset lookup.
    private readonly ulong[] _chunkPrefixSums;
    // -------------------------------------------------------------------------
    // Fields
    // -------------------------------------------------------------------------

    private readonly Stream _source;

    // Logical read position within the decoded (uncompressed) payload.
    private long _position;

    // -------------------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Reads the container header from <paramref name="source" /> and prepares the
    ///     prefix-sum table for O(1) chunk-offset lookup.
    /// </summary>
    /// <param name="source">Raw file stream, positioned at byte 0. Not disposed by this class.</param>
    /// <param name="blowfishKey">Blowfish key for this game's archives.</param>
    public ContainerStream(Stream source, string blowfishKey, IChunkCache? cache = null)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _blowfishKey = blowfishKey ?? throw new ArgumentNullException(nameof(blowfishKey));
        _cache = cache ?? new SinglePageCache();

        using BinaryReader reader = new(source, Encoding.UTF8, true);

        // ---- Container magic ----
        ContainerMagic = (ContainerMagic)reader.ReadUInt32();
        ArchiveFlags flags = ArchiveFlags.None;

        switch (ContainerMagic)
        {
            case ContainerMagic.TTCN:
                // No encryption, no compression — nothing to add.
                break;

            case ContainerMagic.TTCE:
                flags |= ArchiveFlags.IsEncrypted | ArchiveFlags.IsRawDeflateCompressed;
                break;

            case ContainerMagic.TTCZ:
                flags |= ArchiveFlags.IsRawDeflateCompressed;
                break;

            case ContainerMagic.TTCe:
            case ContainerMagic.TTCz:
            {
                if (ContainerMagic is ContainerMagic.TTCe)
                {
                    flags |= ArchiveFlags.IsEncrypted;
                }

                uint compressionType = reader.ReadUInt32();
                flags |= compressionType switch
                {
                    0 => ArchiveFlags.IsRawDeflateCompressed,
                    1 => ArchiveFlags.IsOodleCompressed,
                    _ => throw new NotSupportedException(
                        $"[ContainerStream] Unknown compression type {compressionType}.")
                };
                break;
            }

            default:
                throw new NotSupportedException(
                    $"[ContainerStream] Unknown container version 0x{(uint)ContainerMagic:X8}.");
        }

        Flags = flags;

        if (flags.IsCompressed())
        {
            ChunkSize = reader.ReadUInt32();

            ChunkCount = reader.ReadUInt32();
            ChunkBlockSizes = new ulong[ChunkCount];

            // The on-disk table stores ChunkCount + 1 cumulative absolute offsets.
            // Element 0 is the start of the first chunk (always 0 relative to payload start).
            // We derive per-chunk sizes from successive differences.
            ulong prev = reader.ReadUInt64(); // always 0
            for (int i = 0; i < ChunkCount; i++)
            {
                ulong next = reader.ReadUInt64();
                ChunkBlockSizes[i] = next - prev;
                prev = next;
            }

            // Build prefix-sum table for O(1) chunk-offset lookup.
            _chunkPrefixSums = BuildPrefixSums(ChunkBlockSizes, ChunkCount);

            Length = ChunkCount * ChunkSize;
        }
        else
        {
            Length = reader.ReadInt64(); // uncompressed container payload size — unused
            ChunkBlockSizes = [];
            _chunkPrefixSums = [0L];
        }

        PayloadStart = source.Position; // first byte of TTA2/3/4 payload
    }

    // -------------------------------------------------------------------------
    // Properties exposed to TTArchive2
    // -------------------------------------------------------------------------

    /// <summary>Four-byte container magic read from the file header.</summary>
    public ContainerMagic ContainerMagic { get; }

    /// <summary>Encryption and compression flags derived from the container magic.</summary>
    public ArchiveFlags Flags { get; }

    /// <summary>Decompressed size of each chunk in bytes. Zero for uncompressed containers.</summary>
    public uint ChunkSize { get; }

    /// <summary>Number of compressed chunks. Zero for uncompressed containers.</summary>
    public uint ChunkCount { get; }

    /// <summary>Compressed byte-length of each chunk. Empty for uncompressed containers.</summary>
    public ulong[] ChunkBlockSizes { get; }

    /// <summary>
    ///     Byte offset in the raw source stream where the TTA2/3/4 payload begins
    ///     (immediately after the container header).
    /// </summary>
    public long PayloadStart { get; }

    // -------------------------------------------------------------------------
    // Stream overrides
    // -------------------------------------------------------------------------

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length { get; }

    /// <inheritdoc />
    public override long Position
    {
        get => _position;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _position = value;
        }
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        _position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _position + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };
        return _position;
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
        => Read(buffer.AsSpan(offset, count));

    /// <inheritdoc />
    public override int Read(Span<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        // ---- Uncompressed — backed by MemoryStream ----
        if (!Flags.IsCompressed())
        {
            _source.Position = PayloadStart + _position;
            int bytesToRead = (int)Math.Min(buffer.Length, Length - _position);
            if (bytesToRead <= 0)
            {
                return 0;
            }

            Span<byte> temp = buffer.Slice(0, bytesToRead);
            int read = _source.Read(temp);
            if (read == 0)
            {
                return 0;
            }

            if (Flags.IsEncrypted())
            {
                Blowfish bf = new(_blowfishKey, 7);
                bf.Decipher(temp, read);
            }

            _position += read;
            return read;
        }

        // ---- Compressed — decode on demand, no caching ----
        // ContainerStream is used only for a single sequential forward pass over the
        // TTA header + name pages. Re-decoding the same chunk twice should never happen
        // in practice, so caching would only waste memory.
        int totalRead = 0;

        while (totalRead < buffer.Length)
        {
            int chunkIndex = (int)(_position / ChunkSize);
            if ((uint)chunkIndex >= ChunkCount)
            {
                break; // past end of payload
            }

            // Use cache to get decoded chunk
            byte[] decoded = _cache.GetOrDecode(chunkIndex, DecodeChunk);

            int inChunk = (int)(_position % ChunkSize);
            int avail = decoded.Length - inChunk;
            if (avail <= 0)
            {
                break;
            }

            int toCopy = Math.Min(avail, buffer.Length - totalRead);
            decoded.AsSpan(inChunk, toCopy).CopyTo(buffer[totalRead..]);

            totalRead += toCopy;
            _position += toCopy;
        }

        return totalRead;
    }

    private byte[] DecodeChunk(int chunkIndex)
    {
        long chunkSrcOffset = PayloadStart + (long)_chunkPrefixSums[chunkIndex];
        _source.Seek(chunkSrcOffset, SeekOrigin.Begin);

        byte[] compressed = new byte[ChunkBlockSizes[chunkIndex]];
        ChunkDecoder.ReadExact(_source, compressed);

        ArchiveInfo tempInfo = new() { Flags = Flags, BlowfishKey = _blowfishKey };
        return ChunkDecoder.DecryptAndDecompress(compressed, ChunkSize, tempInfo);
    }

    // -------------------------------------------------------------------------
    // Write / mutate — not supported
    // -------------------------------------------------------------------------

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
        => throw new NotSupportedException("ContainerStream is read-only. Use ContainerWriter to produce output.");

    /// <inheritdoc />
    public override void SetLength(long value)
        => throw new NotSupportedException("ContainerStream is read-only.");

    /// <inheritdoc />
    public override void Flush()
    {
        /* read-only — no-op */
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    /// <summary>
    ///     Builds a prefix-sum array of length <paramref name="count" /> + 1 from
    ///     <paramref name="sizes" />, enabling O(1) compressed-chunk offset lookup.
    ///     <c>result[i]</c> = sum of <c>sizes[0..i-1]</c>.
    /// </summary>
    private static ulong[] BuildPrefixSums(ulong[] sizes, uint count)
    {
        ulong[] sums = new ulong[count + 1];
        for (int i = 0; i < (int)count; i++)
        {
            sums[i + 1] = sums[i] + sizes[i];
        }

        return sums;
    }

    /// <summary>
    ///     Writes a Telltale Tool Container (TTC*) around the given payload.
    ///     Automatically selects the magic number based on encryption and compression settings.
    /// </summary>
    /// <param name="destination">Stream to write the container to. Will be left open.</param>
    /// <param name="payload">Uncompressed, unencrypted inner data (TTA header + files).</param>
    /// <param name="options">Compression, encryption, chunk size, Blowfish key, etc.</param>
    public static void Create(Stream destination, ReadOnlySpan<byte> payload, ArchiveWriteOptions options)
    {
        using MemoryStream ms = new(payload.ToArray());
        Create(destination, ms, options);
    }

    /// <summary>
    ///     Streaming version – writes a Telltale Tool Container without buffering the entire payload.
    /// </summary>
    /// <param name="destination">Output stream (e.g. FileStream).</param>
    /// <param name="payloadStream">
    ///     Stream containing the uncompressed inner payload. Must support seeking and provide a correct Length.
    /// </param>
    /// <param name="options">Archive options (compression, encryption, chunk size, key).</param>
    public static void Create(Stream destination, Stream payloadStream, ArchiveWriteOptions options)
    {
        if (options.Algorithm == CompressionAlgorithm.Oodle)
        {
            throw new NotSupportedException("Oodle compression not yet implemented.");
        }

        // Choose magic automatically
        ContainerMagic magic = (options.Encrypt, options.Algorithm) switch
        {
            (false, CompressionAlgorithm.None) => ContainerMagic.TTCN,
            (true, CompressionAlgorithm.Deflate) => ContainerMagic.TTCE,
            (false, CompressionAlgorithm.Deflate) => ContainerMagic.TTCZ,
            (true, CompressionAlgorithm.Oodle) => ContainerMagic
                .TTCe, // encryption without compression? Rare, but fallback
            (false, CompressionAlgorithm.Oodle) => ContainerMagic
                .TTCz, // encryption without compression? Rare, but fallback
            _ => throw new NotSupportedException(
                $"Unsupported combination: Encrypt={options.Encrypt}, Algorithm={options.Algorithm}")
        };

        using BinaryWriter writer = new(destination, Encoding.UTF8, true);
        writer.Write((uint)magic);

        if (magic is ContainerMagic.TTCe or ContainerMagic.TTCz)
        {
            uint comprType = options.Algorithm == CompressionAlgorithm.Deflate ? 0u : 1u; // 1 = Oodle
            writer.Write(comprType);
        }

        bool compress = options.Algorithm != CompressionAlgorithm.None;
        Blowfish blowfish = new(options.BlowfishKey, 7);

        if (!compress)
        {
            // Uncompressed container: just length + raw data (optional encryption)
            writer.Write((ulong)payloadStream.Length);
            payloadStream.CopyTo(destination);
            if (options.Encrypt)
            {
                throw new NotSupportedException(
                    "Encryption without compression is not supported by this container type.");
            }

            // Note: plain encryption of whole payload would be possible but no known game uses it.
            return;
        }

        int chunkSize = (int)options.ChunkSize;
        long totalLen = payloadStream.Length;
        int chunkCount = (int)((totalLen + chunkSize - 1) / chunkSize);

        writer.Write(chunkSize);
        writer.Write((uint)chunkCount);

        // Reserve space for the offset table (chunkCount+1 entries, each 8 bytes)
        long offsetTablePos = writer.BaseStream.Position;
        for (int i = 0; i <= chunkCount; i++)
        {
            writer.Write(0L); // placeholder
        }

        // We'll store compressed sizes as we write the chunks, then seek back and write cumulative offsets.
        ulong[] compressedSizes = new ulong[chunkCount];
        byte[] compressedBuffer = new byte[chunkSize + chunkSize / 8 + 512]; // worst-case overhead for compression

        payloadStream.Seek(0, SeekOrigin.Begin);
        for (int i = 0; i < chunkCount; i++)
        {
            int bytesToRead = (int)Math.Min(chunkSize, totalLen - payloadStream.Position);
            Span<byte> chunk = new byte[bytesToRead];
            if (payloadStream.Read(chunk) != bytesToRead)
            {
                throw new EndOfStreamException("Unexpected end of payload stream.");
            }

            int compressedLen = TelltaleArchiveUtilities.CompressBlock(chunk, options.Algorithm, compressedBuffer);
            byte[] finalData = compressedBuffer.AsSpan(0, compressedLen).ToArray();
            if (options.Encrypt)
            {
                blowfish.Encipher(finalData, finalData.Length);
            }

            compressedSizes[i] = (ulong)finalData.Length;
            writer.Write(finalData);
        }

        // Go back and write the cumulative offset table
        long endPos = writer.BaseStream.Position;
        writer.BaseStream.Seek(offsetTablePos, SeekOrigin.Begin);
        ulong cumulative = 0;
        writer.Write(cumulative);
        for (int i = 0; i < chunkCount; i++)
        {
            cumulative += compressedSizes[i];
            writer.Write(cumulative);
        }

        writer.BaseStream.Seek(endPos, SeekOrigin.Begin);
    }
}

using System.IO.Compression;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using TelltaleToolKit.Encryption;
using TelltaleToolKit.IO.Archives.Formats;
using TelltaleToolKit.IO.Caching;

namespace TelltaleToolKit.IO.Streams;

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
    // Length = NumPages + 1 so _chunkPrefixSums[NumPages] = total compressed payload size.
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
    public ContainerStream(Stream source, string? blowfishKey = null, IChunkCache? cache = null)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _cache = cache ?? new LruChunkCache();

        using BinaryReader reader = new(source, Encoding.UTF8, true);

        // ---- Container magic ----
        ContainerMagic = (ContainerMagic)reader.ReadUInt32();
        if (!Enum.IsDefined(typeof(ContainerMagic), ContainerMagic))
        {
            throw new NotSupportedException($"[ContainerStream] Unknown container magic {ContainerMagic}.");
        }

        Compression.Mode algorithm = ContainerMagic switch
        {
            ContainerMagic.TTCN => Compression.Mode.None,
            ContainerMagic.TTCe or ContainerMagic.TTCz => ReadCompressionType(reader),
            _ => Compression.Mode.Deflate
        };

        bool isEncrypted = ContainerMagic is ContainerMagic.TTCE or ContainerMagic.TTCe;

        if (isEncrypted && blowfishKey is null)
        {
            throw new ArgumentNullException(nameof(blowfishKey),
                "Blowfish key is required for encrypted containers.");
        }

        Params = new ContainerStreamParams
        {
            Encrypt = isEncrypted,
            BlowfishKey = blowfishKey!,
            Algorithm = algorithm
        };

        if (Params.Algorithm != Compression.Mode.None)
        {
            WindowSize = reader.ReadUInt32();

            NumPages = reader.ReadUInt32();
            ChunkBlockSizes = new ulong[NumPages];

            // The on-disk table stores NumPages + 1 cumulative absolute offsets.
            // Element 0 is the start of the first chunk (always 0 relative to payload start).
            // We derive per-chunk sizes from successive differences.
            ulong prev = reader.ReadUInt64(); // always 0
            for (int i = 0; i < NumPages; i++)
            {
                ulong next = reader.ReadUInt64();
                ChunkBlockSizes[i] = next - prev;
                prev = next;
            }

            // Build prefix-sum table for O(1) chunk-offset lookup.
            _chunkPrefixSums = BuildPrefixSums(ChunkBlockSizes, NumPages);

            Length = NumPages * WindowSize;
        }
        else
        {
            Length = reader.ReadInt64(); // uncompressed container payload size — unused
            ChunkBlockSizes = [];
            _chunkPrefixSums = [0L];
        }

        PayloadStart = source.Position; // first byte of TTA2/3/4 payload
    }

    private static Compression.Mode ReadCompressionType(BinaryReader reader)
    {
        uint compressionType = reader.ReadUInt32();
        return compressionType switch
        {
            0 => Compression.Mode.Deflate,
            1 => Compression.Mode.Oodle,
            _ => throw new NotSupportedException(
                $"[ContainerStream] Unknown compression type {compressionType}.")
        };
    }

    // -------------------------------------------------------------------------
    // Properties exposed to TTArchive2
    // -------------------------------------------------------------------------

    /// <summary>Four-byte container magic read from the file header.</summary>
    public ContainerMagic ContainerMagic { get; }

    /// <summary>Encryption and compression flags derived from the container magic.</summary>
    public ContainerStreamParams Params { get; }

    /// <summary>Decompressed size of each chunk in bytes. Zero for uncompressed containers.</summary>
    public uint WindowSize { get; }

    /// <summary>Number of compressed chunks. Zero for uncompressed containers.</summary>
    public uint NumPages { get; }

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
        if (Params.Algorithm is Compression.Mode.None)
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

            if (Params.Encrypt)
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
            int chunkIndex = (int)(_position / WindowSize);
            if ((uint)chunkIndex >= NumPages)
            {
                break; // past end of payload
            }

            // Use cache to get decoded chunk
            byte[] decoded = _cache.GetOrDecode(chunkIndex, DecodeChunk);

            int inChunk = (int)(_position % WindowSize);
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
        Compression.ReadExact(_source, compressed);

        if (Params.Encrypt)
        {
            Blowfish bf = new(Params.BlowfishKey, 7);
            bf.Decipher(compressed.AsSpan(), compressed.Length);
        }

        return DecompressBlock(compressed, (int)WindowSize, Params.Algorithm);
    }

    public static byte[] DecompressBlock(byte[] compressedData, int expectedSize,
        Compression.Mode compression)
    {
        // This is a hack? In TWD:DE, 401_txmesh, there's a page which is the same size as the expected size (65535)
        // C#'s raw deflate fails.
        // ttarchext returns the same data:
        // https://github.com/infernokun/TelltaleGamesExtractionGUI/blob/fec9fc1a70b545bcc67062fedc3fe3f7cd0d3e1b/bin/Debug/net6.0-windows/ttarchext/ttarchext.c#L1512
        if (compressedData.Length == expectedSize)
        {
            return compressedData;
        }

        return compression switch
        {
            Compression.Mode.Deflate => Decompress(ms => new DeflateStream(ms, CompressionMode.Decompress)),
            Compression.Mode.Zlib => Decompress(ms => new InflaterInputStream(ms)),
            Compression.Mode.Oodle => throw new NotSupportedException("Oodle compression is not supported yet."),
            // No compression, return the original data
            _ => compressedData
        };

        // Local function to handle decompression
        byte[] Decompress(Func<Stream, Stream> streamFactory)
        {
            using MemoryStream outputStream = new(expectedSize);
            using Stream decompressStream = streamFactory(new MemoryStream(compressedData));
            decompressStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }
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
    public static void Create(Stream destination, ReadOnlySpan<byte> payload, ContainerStreamParams options)
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
    public static void Create(Stream destination, Stream payloadStream, ContainerStreamParams options)
    {
        if (options.Algorithm == Compression.Mode.Oodle)
        {
            throw new NotSupportedException("Oodle compression not yet implemented.");
        }

        // Choose magic automatically
        ContainerMagic magic = (options.Encrypt, options.Algorithm) switch
        {
            (false, Compression.Mode.None) => ContainerMagic.TTCN,
            (true, Compression.Mode.Deflate) => ContainerMagic.TTCE,
            (false, Compression.Mode.Deflate) => ContainerMagic.TTCZ,
            (true, Compression.Mode.Oodle) => ContainerMagic
                .TTCe, // encryption without compression? Rare, but fallback
            (false, Compression.Mode.Oodle) => ContainerMagic
                .TTCz, // encryption without compression? Rare, but fallback
            _ => throw new NotSupportedException(
                $"Unsupported combination: Encrypt={options.Encrypt}, Compression={options.Algorithm}")
        };

        using BinaryWriter writer = new(destination, Encoding.UTF8, true);
        writer.Write((uint)magic);

        if (magic is ContainerMagic.TTCe or ContainerMagic.TTCz)
        {
            uint comprType = options.Algorithm == Compression.Mode.Deflate ? 0u : 1u; // 1 = Oodle
            writer.Write(comprType);
        }

        bool compress = options.Algorithm != Compression.Mode.None;
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

            int compressedLen = Compression.Compress(chunk, options.Algorithm, compressedBuffer);
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

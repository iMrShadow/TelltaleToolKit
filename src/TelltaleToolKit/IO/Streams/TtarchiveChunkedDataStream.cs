using System.IO.Compression;
using TelltaleToolKit.IO.Archives;
using TelltaleToolKit.Utility.Blowfish;
using TelltaleToolKit.Utility.Caching;

namespace TelltaleToolKit.IO.Streams;

/// <summary>
///     Presents a continuous, seekable stream of decompressed file data from a
///     TTArchive compressed chunk table (version>=6, filesMode=2).
/// </summary>
internal sealed class TtarchiveChunkedDataStream : Stream
{
    private readonly Blowfish _blowfish;
    private readonly IChunkCache _cache;
    private readonly ulong[] _chunkPrefixSums;
    private readonly uint _chunkSize;
    private readonly ulong[] _chunkSizes;
    private readonly long _dataStart;
    private readonly bool _isEncrypted;
    private readonly Stream _source;
    public Compression.Mode Compression { get; private set; }
    private long _position;

    public TtarchiveChunkedDataStream(
        Stream source,
        long dataStart,
        uint chunkSize,
        ulong[] chunkSizes,
        Compression.Mode compressionMode,
        string blowfishKey,
        int archiveVersion,
        bool isEncrypted, IChunkCache? cache = null)
    {
        _source = source;
        _dataStart = dataStart;
        _chunkSize = chunkSize;
        _chunkSizes = chunkSizes;
        _isEncrypted = isEncrypted;
        _blowfish = new Blowfish(blowfishKey, archiveVersion);
        _cache = cache ?? new SinglePageCache();
        Compression = compressionMode;
        // Build prefix sums for O(1) chunk offset lookup
        _chunkPrefixSums = new ulong[chunkSizes.Length + 1];
        for (int i = 0; i < chunkSizes.Length; i++)
        {
            _chunkPrefixSums[i + 1] = _chunkPrefixSums[i] + chunkSizes[i];
        }
    }

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;

    // Total decompressed length of the file-data region
    public override long Length => _chunkSizes.Length * _chunkSize;

    public override long Position
    {
        get => _position;
        set
        {
            if (value < 0 || value > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _position = value;
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
        => Read(buffer.AsSpan(offset, count));

    public override int Read(Span<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        long remaining = Length - _position;
        if (remaining <= 0)
        {
            return 0;
        }

        int toRead = (int)Math.Min(buffer.Length, remaining);

        int totalRead = 0;
        while (totalRead < toRead)
        {
            int chunkIdx = (int)(_position / _chunkSize);
            if (chunkIdx >= _chunkSizes.Length)
            {
                break;
            }

            byte[] decompressed = _cache.GetOrDecode(chunkIdx, DecodeChunk);

            int inChunk = (int)(_position % _chunkSize);
            int avail = decompressed.Length - inChunk;
            int copyNow = Math.Min(avail, toRead - totalRead);

            decompressed.AsSpan(inChunk, copyNow).CopyTo(buffer[totalRead..]);
            totalRead += copyNow;
            _position += copyNow;
        }

        return totalRead;
    }

    private byte[] DecodeChunk(int chunkIdx)
    {
        // Seek to the compressed chunk
        long offset = _dataStart + (long)_chunkPrefixSums[chunkIdx];
        _source.Seek(offset, SeekOrigin.Begin);

        byte[] compressed = new byte[_chunkSizes[chunkIdx]];
        IO.Compression.ReadExact(_source, compressed);

        // Decrypt chunk if needed (per‑chunk, not per‑file)
        if (_isEncrypted)
        {
            _blowfish.Decipher(compressed.AsSpan(), compressed.Length);
        }

        if (Compression is IO.Compression.Mode.None)
        {
            // Detect the compression
            Compression = IO.Compression.DetectMode(compressed);
        }

        return IO.Compression.Decompress(compressed, Compression, (int)_chunkSize);
    }

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

    public override void Flush()
    {
    }

    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}

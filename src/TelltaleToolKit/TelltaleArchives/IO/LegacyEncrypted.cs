using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.TelltaleArchives.IO;

/// <summary>
///     Stream wrapper that decrypts TTArchive per‑file data on the fly.
///     For simplicity, it reads the entire file into memory – these files are small
///     (config files, scripts) so the overhead is acceptable.
/// </summary>
internal sealed class LegacyEncrypted : Stream
{
    private readonly Blowfish _blowfish;
    private readonly Stream _inner;

    public LegacyEncrypted(Stream inner, Blowfish blowfish)
    {
        _inner = inner;
        _blowfish = blowfish;
        // Read the whole file, decrypt it, and keep the result in memory.
        using MemoryStream ms = new();
        _inner.CopyTo(ms);
        // _decryptedData = ms.ToArray();
        // TelltaleArchiveUtilities.DecryptFile(_decryptedData, _blowfish);
    }

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;

    public override long Length { get; }
    //   public override long Length => _decryptedData.Length;

    public override long Position { get; set; }

    public override int Read(byte[] buffer, int offset, int count)
        => Read(buffer.AsSpan(offset, count));

    public override int Read(Span<byte> buffer)
    {
        long remaining = Length - Position;
        if (remaining <= 0)
        {
            return 0;
        }

        int toRead = (int)Math.Min(buffer.Length, remaining);
        //  _decryptedData.AsSpan((int)_position, toRead).CopyTo(buffer);
        Position += toRead;
        return toRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => Position + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };
        return Position;
    }

    public override void Flush()
    {
    }

    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}

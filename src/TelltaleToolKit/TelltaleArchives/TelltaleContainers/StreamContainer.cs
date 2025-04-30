namespace TelltaleToolKit.TelltaleArchives.TelltaleContainers;

public class DataStreamContainer : Stream
{
    // Public properties for settings
    public bool IsCompressed { get; }
    public bool IsEncrypted { get; }
    public CompressionLibrary CompressionLibrary { get; }

    // Internal state
    private Stream _sourceStream;
    private long _containerSize;
    private long _position;

    // Constructors: just take what you need
    public DataStreamContainer(Stream source, bool isCompressed = false, bool isEncrypted = false, CompressionLibrary clib = CompressionLibrary.None)
    {
        _sourceStream = source;
        IsCompressed = isCompressed;
        IsEncrypted = isEncrypted;
        CompressionLibrary = clib;
        _containerSize = source.Length;
        _position = 0;
    }

    // Implement Stream abstract members
    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;
    public override long Length => _containerSize;
    public override long Position
    {
        get => _position;
        set => _position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        // Add decompression/decryption as needed here
        _sourceStream.Position = _position;
        int bytesRead = _sourceStream.Read(buffer, offset, count);
        _position += bytesRead;
        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin: _position = offset; break;
            case SeekOrigin.Current: _position += offset; break;
            case SeekOrigin.End: _position = _containerSize + offset; break;
        }
        return _position;
    }

    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    public override void Flush() { }
}

public enum CompressionLibrary
{
    None,
    // Add more as needed
}
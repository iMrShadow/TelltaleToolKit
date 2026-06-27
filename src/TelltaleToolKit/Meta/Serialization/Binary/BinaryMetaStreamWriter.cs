using System.Text;
using TelltaleToolKit.Encryption;
using TelltaleToolKit.IO.Streams;

namespace TelltaleToolKit.Meta.Serialization.Binary;

public sealed class BinaryMetaStreamWriter : MetaStream
{
    private readonly Stream _outputStream;
    private bool _closed;

    public BinaryMetaStreamWriter(Stream outputStream, MetaStreamParams @params)
    {
        _outputStream = outputStream;
        Params = @params;
        Params.SerializedSymbols.Clear();

        Sections[0].Stream = new MemoryStream(0x100);
        Sections[0].IsEnabled = true;
        SetSection(SectionType.Default);
    }

    private BinaryWriter Writer { get; set; } = null!;

    public override MetaStreamMode Mode => MetaStreamMode.Write;

    private void WriteHeader()
    {
        SetSection(SectionType.Header);
        this.Write((uint)Params.ResolveMagic());
        uint streamVersion = Params.StreamVersion;

        if (streamVersion >= 4)
        {
            uint defaultSize = (uint)(GetSection(SectionType.Default).Stream?.Length ?? 0);
            uint debugSize = (uint)(GetSection(SectionType.Debug).Stream?.Length ?? 0);
            uint asyncSize = (uint)(GetSection(SectionType.Async).Stream?.Length ?? 0);

            if (GetSection(SectionType.Default).IsCompressed)
            {
                defaultSize |= 0x80000000;
            }

            if (GetSection(SectionType.Debug).IsCompressed)
            {
                debugSize |= 0x80000000;
            }

            if (GetSection(SectionType.Async).IsCompressed)
            {
                asyncSize |= 0x80000000;
            }

            if (streamVersion >= 5)
            {
                this.Write(defaultSize);
            }

            this.Write(debugSize);
            this.Write(asyncSize);
        }

        Params.VersionInfo = Params.VersionInfo.Distinct().ToList();

        this.Write(Params.VersionInfo.Count);

        foreach (MetaVersionInfo versionInfo in Params.VersionInfo)
        {
            if (streamVersion >= 3)
            {
                this.Write(versionInfo.TypeSymbolCrc);
            }
            else
            {
                this.Write(versionInfo.GetMetaClassType()!.FullTypeName);
            }

            this.Write(versionInfo.VersionCrc);
        }
    }

    protected override bool SetSection(SectionType newSection)
    {
        var section = Sections[(int)newSection];
        if (section.Stream is not null)
        {
            Writer = new BinaryWriter(section.Stream, Encoding.UTF8, true);
            _currentSection = newSection;
            return true;
        }

        if (!section.IsEnabled)
        {
            return false;
        }

        section.Stream =  new MemoryStream(0x4000);
        Writer = new BinaryWriter(section.Stream, Encoding.UTF8, true);
        _currentSection = newSection;
        return true;
    }

    public override void BeginBlock()
    {
        long position = CurrentSection.Stream!.Position;
        CurrentSection.Blocks.Push(position);
        this.Write(0);
    }

    public override void EndBlock()
    {
        long position = CurrentSection.Blocks.Pop();

        long currentPosition = CurrentSection.Stream!.Position;
        int blockSize = (int)(currentPosition - position);

        CurrentSection.Stream.Seek(position, SeekOrigin.Begin);
        this.Write(blockSize);
        CurrentSection.Stream.Seek(currentPosition, SeekOrigin.Begin);
    }

    public override void Serialize(ref bool value) => Writer.Write(value ? '1' : '0');

    public override void Serialize(ref float value) => Writer.Write(value);

    public override void Serialize(ref double value) => Writer.Write(value);

    public override void Serialize(ref short value) => Writer.Write(value);

    public override void Serialize(ref int value) => Writer.Write(value);

    public override void Serialize(ref long value) => Writer.Write(value);

    public override void Serialize(ref ushort value) => Writer.Write(value);

    public override void Serialize(ref uint value) => Writer.Write(value);

    public override void Serialize(ref ulong value) => Writer.Write(value);

    public override void Serialize(ref string value)
    {
        byte[] strBytes = Encoding.UTF8.GetBytes(value);
        Writer.Write(strBytes.Length);
        Writer.Write(strBytes);
    }

    public override void Serialize(ref char value) => Writer.Write(value);

    public override void Serialize(ref byte value) => Writer.Write(value);

    public override void Serialize(ref sbyte value) => Writer.Write(value);

    public override void Serialize(byte[] values, int offset, int count) => Writer.Write(values, offset, count);

    private void FinalizeStream()
    {
        if (_closed)
        {
            throw new ObjectDisposedException(nameof(BinaryMetaStreamWriter));
        }

        uint streamVersion = Params.StreamVersion;
        bool canCompress = streamVersion >= 4;
        // Telltale does not compress the header section, so we can skip it.
        for (int i = 1; i <= 3; i++)
        {
            SectionInfo section = Sections[i];
            if (section.Stream is { Length: > 0 } && Params.Compress && canCompress)
            {
                ContainerStreamParams options = new();
                MemoryStream outStream = new(0x10000);
                ContainerStream.Create(outStream, section.Stream, options);
                section.Stream.Dispose();
                section.Stream = outStream;
                section.CompressedSize = outStream.Length;
                section.IsCompressed = true;
            }
            else
            {
                section.CompressedSize = section.Stream?.Length ?? 0;
                section.IsCompressed = false;
            }
        }
    }

    public override void Close()
    {
        if (_closed)
        {
            return;
        }

        FinalizeStream();
        WriteHeader();

        // Iterate each section (header, default, debug, async) and copy its data to the output stream.
        foreach (SectionInfo section in Sections)
        {
            if (section.Stream is null)
            {
                continue;
            }

            section.Stream.Seek(0, SeekOrigin.Begin);
            section.Stream.CopyTo(_outputStream);
        }

        if (Params is { Encrypt: true, StreamVersion: <= 3 })
        {
            MetaStreamMagic targetMagic = Params.ResolveMagic();

            if (!_outputStream.CanRead || !_outputStream.CanSeek)
                throw new InvalidOperationException(
                    "Legacy encryption requires a readable and seekable output stream (e.g. MemoryStream or FileStream opened with FileAccess.ReadWrite). " +
                    "The encryption is applied in-place after writing.");

            LegacyEncryption.Encrypt(_outputStream, targetMagic, Params.Workspace!.Blowfish);
        }

        _closed = true;
        Dispose(true);
    }
}

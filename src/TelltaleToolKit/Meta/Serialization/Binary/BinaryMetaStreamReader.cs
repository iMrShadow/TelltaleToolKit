using System.Text;
using TelltaleToolKit.Hashing;
using TelltaleToolKit.IO.Streams;
using TelltaleToolKit.Meta.Reflection;

namespace TelltaleToolKit.Meta.Serialization.Binary;

public sealed class BinaryMetaStreamReader : MetaStream
{
    private BinaryReader Reader = null!;

    public BinaryMetaStreamReader(Stream inputStream, Workspace? workspace = null)
    {
        BaseStream = inputStream;
        Params.Workspace = workspace;

        bool isValid = ReadHeader();

        if (!isValid)
        {
            throw new InvalidDataException("[BinaryMetaStreamReader] Invalid MetaStream.");
        }

        long offset = Sections[0].CompressedSize;
        for (int i = 1; i <= 3; i++)
        {
            //for each section (default,async,debug)
            SectionInfo currentSect = Sections[i];
            if (currentSect.CompressedSize > 0)
            {
                currentSect.Stream = new SubStream(Sections[0].Stream, offset, currentSect.CompressedSize);
                if (currentSect.IsCompressed)
                {
                    ContainerStream cs = new(currentSect.Stream);
                    currentSect.Stream = cs;
                    SetSection((SectionType)i);

                    // No idea why this is skipped
                    this.ReadUInt64();
                }

                offset += currentSect.CompressedSize;
            }
        }

        SetSection(SectionType.Default);
    }

    public override MetaStreamMode Mode => MetaStreamMode.Read;

    private bool ReadHeader()
    {
        if (BaseStream.Length < 4)
        {
            Toolkit.Instance.Logger.LogError("[BinaryMetaStreamReader] Invalid header: Stream too short.");
            return false;
        }

        // Setup the header section
        Sections[(int)SectionType.Header].Stream = BaseStream;
        Reader = new BinaryReader(Sections[(int)SectionType.Header].Stream, Encoding.UTF8, true);
        SetSection(SectionType.Header);

        // Read the FourCC
        MetaStreamMagic magic = (MetaStreamMagic)this.ReadUInt32();
        Params.StreamVersion = magic.GetMetaStreamVersion();
        uint streamVersion = Params.StreamVersion;

        if (Params.StreamVersion == 0)
        {
            Toolkit.Instance.Logger.LogError($"[BinaryMetaStreamReader] Not a valid meta stream version: {magic}");
            return false;
        }

        long defaultSize = 0, debugSize = 0, asyncSize = 0;

        if (streamVersion >= 4)
        {
            // MSV5 + MSV6
            if (streamVersion >= 5)
            {
                defaultSize = this.ReadUInt32();
            }

            debugSize = this.ReadUInt32();
            asyncSize = this.ReadUInt32();

            if ((defaultSize & 0x80000000) != 0)
            {
                defaultSize &= 0x7FFFFFFF;
                Sections[1].IsCompressed = true;
            }

            if ((debugSize & 0x80000000) != 0)
            {
                debugSize &= 0x7FFFFFFF;
                Sections[2].IsCompressed = true;
            }

            if ((asyncSize & 0x80000000) != 0)
            {
                asyncSize &= 0x7FFFFFFF;
                Sections[3].IsCompressed = true;
            }
        }
        else if (magic is not MetaStreamMagic.Mbin and not MetaStreamMagic.Mtre)
        {
            Sections[(int)SectionType.Header].IsEnabled = true;
            Params.Encrypt = true;
            // For MCOM, there is an extra 4-byte field to skip
            if (magic is MetaStreamMagic.Mcom or MetaStreamMagic.EncryptedMcom)
            {
                _ = this.ReadUInt32(); // Actually, this might be the fully decompressed data size.

                if (magic is MetaStreamMagic.Mcom)
                {
                    Params.Encrypt = false;
                    /* TODO: This is not a priority, because Telltale haven't shipped any games with MCOM or encrypted MCOM.
                      But it would be nice to support them in the future.
                      In TWDS1, there's a function called "ReadUncompressAndStoreInMemory" which reads and decompresses the whole stream in memory.
                      It uses zlib compression.
                      This means MCOM is just a compressed version of the entire file.
                      While the encrypted version is still compressed, but the decompressed data is encrypted.
                   */
                }

                Toolkit.Instance.Logger.LogError("[BinaryMetaStreamReader] Mcom or EncryptedMcom is not supported.");
                return false;
            }

            if (Params.Workspace is null)
            {
                Toolkit.Instance.Logger.LogError("[BinaryMetaStreamReader] Workspace is not set for encrypted streams.");
                return false;
            }

            // Create a LegacyEncryptedStream that starts after the magic (and possible extra)
            LegacyEncryptedStream encryptedStream = new(BaseStream, magic.GetMetaStreamVersion(),
                GetPosition(),
                Params.Workspace.Blowfish);

            // Replace the underlying stream and reinitialize the BinaryReader
            Sections[0].Stream = encryptedStream;
            Reader = new BinaryReader(Sections[0].Stream, Encoding.UTF8, true);
        }

        // Read the number of serialized classes
        uint numVers = this.ReadUInt32();

        if (numVers >= 1024)
        {
            Toolkit.Instance.Logger.LogError("[BinaryMetaStreamReader] Too many serialized classes.");
            return false;
        }

        Params.VersionInfo.Capacity = (int)numVers;
        for (int i = 0; i < numVers; i++)
        {
            MetaVersionInfo verInfo = new();

            if (magic.GetMetaStreamVersion() >= 3) // uses hashed types
            {
                verInfo.TypeSymbolCrc = this.ReadUInt64();
            }
            else
            {
                string typeName = this.ReadString();
                verInfo.TypeSymbolCrc = Crc64.Compute(MetaClassType.GetStrippedTypeName(typeName));
            }

            verInfo.VersionCrc = this.ReadUInt32();
            Params.VersionInfo.Add(verInfo);
        }

        long headerSize = Sections[(int)SectionType.Header].Stream!.Position;

        defaultSize = Params.StreamVersion >= 5
            ? defaultSize
            : BaseStream.Length - headerSize - debugSize - asyncSize;

        // The encrypted header is lost in the process, so we need to add it back
        if (Params.Encrypt)
        {
            defaultSize -= 4;
        }

        Sections[0].CompressedSize = headerSize;
        Sections[1].CompressedSize = defaultSize;
        Sections[2].CompressedSize = debugSize;
        Sections[3].CompressedSize = asyncSize;

        return true;
    }

    protected override bool SetSection(SectionType newSection)
    {
        var section = Sections[(int)newSection];
        if (section.Stream is null)
        {
            return false;
        }

        Reader = new BinaryReader(section.Stream, Encoding.UTF8, true);
        _currentSection = newSection;
        return true;
    }

    public override void BeginBlock()
    {
        int size = this.ReadInt32();
        long expectedPosition = Sections[(int)_currentSection].Stream!.Position + size - sizeof(int);
        Sections[(int)_currentSection].Blocks.Push(expectedPosition);
    }

    public override void EndBlock()
    {
        SectionInfo currentSectionInfo = Sections[(int)_currentSection];
        long expectedPosition = currentSectionInfo.Blocks.Pop();
        long currentPosition = currentSectionInfo.Stream!.Position;
        if (expectedPosition == currentPosition)
        {
            return;
        }

        // Telltale Tool skips the block if it's not read fully.
        // I have no idea how they got there in the first place, but I saw one such file (ui_mainmenu_background.scene) which benefits from this.
        // Previously, I used to throw exceptions since I thought it was impossible, but this seems better for stability purposes.
        // This matches the implementation of the engine behaviour.

        if (expectedPosition < currentPosition)
        {
            Toolkit.Instance.Logger.LogWarning(
                $"[BinaryMetaStreamReader] Invalid data position! Current position ahead of expected position! Current Position: {currentPosition}. Expected Position: {expectedPosition}.");
        }

        //  throw new InvalidDataException($"Invalid data position! Current Position: {currentPosition}. Expected Position: {expectedPosition}.");

        currentSectionInfo.Stream.Position = expectedPosition;
        currentSectionInfo.CompressedSize -= (int)(currentPosition - expectedPosition);
        // #if DEBUG
        // throw new InvalidDataException($"Invalid data position! Current Position: {currentPosition}. Expected Position: {expectedPosition}.");
        // #endif
    }

    public override void Serialize(ref bool value) =>
        value = Reader.ReadChar() switch
        {
            '1' => true,
            '0' => false,
            _ => throw new InvalidBooleanException($"[BinaryMetaStreamReader] Invalid boolean at position: {Reader.BaseStream.Position}!")
        };

    public override void Serialize(ref float value) => value = Reader.ReadSingle();

    public override void Serialize(ref double value) => value = Reader.ReadDouble();

    public override void Serialize(ref short value) => value = Reader.ReadInt16();

    public override void Serialize(ref int value) => value = Reader.ReadInt32();

    public override void Serialize(ref long value) => value = Reader.ReadInt64();

    public override void Serialize(ref ushort value) => value = Reader.ReadUInt16();

    public override void Serialize(ref uint value) => value = Reader.ReadUInt32();

    public override void Serialize(ref ulong value) => value = Reader.ReadUInt64();

    public override void Serialize(ref string value)
    {
        int strLength = Reader.ReadInt32();

        if (strLength < 0 || strLength > 0x100000)
        {
            throw new ArgumentOutOfRangeException(nameof(strLength));
        }

        byte[] buffer = new byte[strLength];
        int realLength = Reader.Read(buffer, 0, strLength);

        if (realLength != strLength)
        {
            throw new EndOfStreamException($"Expected {strLength} bytes but read {realLength}.");
        }

        value = Encoding.UTF8.GetString(buffer, 0, realLength);
    }

    public override void Serialize(ref char value) => value = Reader.ReadChar();

    public override void Serialize(ref byte value) => value = Reader.ReadByte();

    public override void Serialize(ref sbyte value) => value = Reader.ReadSByte();

    public override void Serialize(byte[] values, int offset, int count) => Reader.Read(values, offset, count);

    public override void Close() => Dispose(true);
}

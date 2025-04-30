using System.Text;
using SubstreamSharp;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization.Binary;

public sealed class MetaStreamReader : MetaStream
{
    public MetaStreamReader(Stream inputStream)
    {
        Reader = new BinaryReader(inputStream, Encoding.UTF8, true);
        UnderlyingStream = inputStream;
        SerializeMetaHeader();
    }

    private BinaryReader Reader { get; set; }

    private void ValidateMetaHeader()
    {
        if (UnderlyingStream.Length < 4)
            throw new InvalidDataException("Invalid MetaHeader: Stream too short.");

        UnderlyingStream.Position = 0;
        Configuration.Version = (MetaStreamVersion)this.ReadUInt32();

        if (!Enum.IsDefined(Configuration.Version))
            throw new InvalidDataException($"Not a valid meta stream version: {Configuration.Version}");
    }


    public override void SerializeMetaHeader()
    {
        ValidateMetaHeader();

        uint mainSize = 0, debugSize = 0, asyncSize = 0;

        switch (Configuration.Version)
        {
            case MetaStreamVersion.Mbes:
                throw new NotSupportedException("MBES is not supported yet.");
            // TODO optional compressed sections (set top bit of size to 1)
            case MetaStreamVersion.Msv4:
                mainSize = this.ReadUInt32();
                asyncSize = this.ReadUInt32();
                break;
            case MetaStreamVersion.Msv6
                or MetaStreamVersion.Msv5:
                mainSize = this.ReadUInt32();
                debugSize = this.ReadUInt32();
                asyncSize = this.ReadUInt32();
                break;
        }

        // Read the number of serialized classes
        uint metaClassNum = this.ReadUInt32();

        for (var i = 0; i < metaClassNum; i++)
        {
            uint size = this.ReadUInt32();

            // Read the type hash or name (256 is a good number to use for the size of the name)
            Configuration.AreSymbolsHashed = size > 255;

            Reader.BaseStream.Seek(-4, SeekOrigin.Current);

            MetaClassType type = this.ReadMetaClassType();
            uint crc32 = this.ReadUInt32();

            // If the type is not recognized at all
            if (type.FullTypeName == string.Empty)
            {
                if (Configuration.UnregisteredTypes.Count > 0)
                {
                    int lastIndex = Configuration.UnregisteredTypes.Count - 1;
                    (ulong, uint) lastTuple = Configuration.UnregisteredTypes[lastIndex];
                    Configuration.UnregisteredTypes[lastIndex] = (lastTuple.Item1, crc32);
                }

                continue;
            }

            MetaClass? mcs = TTKContext.Instance().GetClass(type.Symbol, crc32);

            if (mcs is not null)
            {
                // If the class description exists, add it. Otherwise TODO
                Configuration.SerializedClasses.Add(mcs);
            }
            else
            {
                Configuration.UnregisteredClasses.Add((type, crc32));
            }
        }

        long currentPosition = Reader.BaseStream.Position;
        if (Configuration.Version is MetaStreamVersion.Msv6 or MetaStreamVersion.Msv5)
        {
            Sections[(int)SectionType.Main].Stream = new Substream(UnderlyingStream, currentPosition, mainSize);
            Sections[(int)SectionType.Debug].Stream =
                new Substream(UnderlyingStream, currentPosition + mainSize, debugSize);
            Sections[(int)SectionType.Async].Stream = new Substream(
                UnderlyingStream,
                currentPosition + mainSize + debugSize,
                asyncSize
            );
        }
        else
        {
            // For older versions, the entire stream is the main section
            Sections[(int)SectionType.Main].Stream = new Substream(
                UnderlyingStream,
                currentPosition,
                UnderlyingStream.Length - currentPosition
            );
            Sections[(int)SectionType.Debug].Stream = new Substream(UnderlyingStream, 0, 0);
            Sections[(int)SectionType.Async].Stream = new Substream(UnderlyingStream, 0, 0);
        }

        InitSerializer();
    }

    protected override void InitSerializer() => Reader = new BinaryReader(CurrentSubstream, Encoding.UTF8, true);

    public override void BeginBlock()
    {
        int size = this.ReadInt32();
        long expectedPosition = GetCurrentSection().Stream.Position + size - sizeof(int);
        GetCurrentSection().Blocks.Push(expectedPosition);
    }

    /// <inheritdoc />
    public override void EndBlock()
    {
        MetaSection currentSection = GetCurrentSection();
        long expectedPosition = currentSection.Blocks.Pop();
        long currentPosition = currentSection.Stream.Position;
        if (expectedPosition == currentPosition)
            return;
        // Telltale Tool skips the block if it's not read fully.
        // I have no idea how they got there in the first place, but I saw one such file (ui_mainmenu_background.scene) which benefits from this.
        // Previously, I used to throw exceptions since I thought it was impossible, but this seems better for stability purposes.
        // This matches the implementation of the engine behaviour.
        GetCurrentSection().Stream.Position = expectedPosition;
        Console.WriteLine(
            $"Warning: Invalid data position! Current Position:{currentPosition}. Expected Position:{expectedPosition}");
    }

    /// <summary>
    /// Skip a block.
    /// This is internally used, even by Telltale themselves.
    /// </summary>
    public void SkipToEndOfCurrentBlock()
    {
        MetaSection currentSection = GetCurrentSection();
        long expectedPosition = currentSection.Blocks.Pop();
        currentSection.Stream.Seek(expectedPosition, SeekOrigin.Begin);
    }

    public override MetaClass? GetMetaClass(Type type) =>
        Configuration.SerializedClasses.FirstOrDefault(tc => tc.ClassType.LinkingType == type);

    public override MetaClass? GetMetaClass(Symbol symbol) =>
        Configuration.SerializedClasses.FirstOrDefault(tc => tc.ClassType.Symbol.Crc64 == symbol.Crc64);

    /// <inheritdoc />
    public override void Serialize(ref bool value)
    {
        value = Reader.ReadChar() switch
        {
            '1' => true,
            '0' => false,
            _ => throw new InvalidBooleanException($"Invalid boolean at position: {Reader.BaseStream.Position}!")
        };
    }

    /// <inheritdoc />
    public override void Serialize(ref float value) => value = Reader.ReadSingle();

    /// <inheritdoc />
    public override void Serialize(ref double value) => value = Reader.ReadDouble();

    /// <inheritdoc />
    public override void Serialize(ref short value) => value = Reader.ReadInt16();

    /// <inheritdoc />
    public override void Serialize(ref int value) => value = Reader.ReadInt32();

    /// <inheritdoc />
    public override void Serialize(ref long value) => value = Reader.ReadInt64();

    /// <inheritdoc />
    public override void Serialize(ref ushort value) => value = Reader.ReadUInt16();

    /// <inheritdoc />
    public override void Serialize(ref uint value) => value = Reader.ReadUInt32();

    /// <inheritdoc />
    public override void Serialize(ref ulong value) => value = Reader.ReadUInt64();


    /// <inheritdoc />
    public override void Serialize(ref string value)
    {
        int strLength = Reader.ReadInt32();

        ArgumentOutOfRangeException.ThrowIfLessThan(strLength, 0x0, nameof(strLength));

        var buffer = new byte[strLength];
        int realLength = Reader.Read(buffer, 0, strLength);

        value = Encoding.UTF8.GetString(buffer, 0, realLength);
    }

    public override void Serialize(ref char value) => value = Reader.ReadChar();

    public override void Serialize(ref byte value) => value = Reader.ReadByte();

    public override void Serialize(ref sbyte value) => value = Reader.ReadSByte();

    public override void Serialize(ref Symbol value)
    {
        if (Configuration.AreSymbolsHashed)
        {
            value = new Symbol(Reader.ReadUInt64());
        }
        else
        {
            var valueSymbolName = string.Empty;
            Serialize(ref valueSymbolName);
            value = new Symbol(valueSymbolName);
        }

        Configuration.SerializedSymbols.Add(value);
    }

    public override void Serialize(ref MetaClassType value)
    {
        if (Configuration.AreSymbolsHashed)
        {
            ulong hash = this.ReadUInt64();
            MetaClassType? type = MetaClassTypeRegistry.GetByHash(hash);

            if (type == null)
            {
                Console.WriteLine($"Unknown MetaClassType!: {hash:X}");
                Configuration.UnregisteredTypes.Add((hash, 0));
            }
            else
            {
                value = type;
            }
        }
        else
        {
            string fullType = this.ReadString();
            value = MetaClassTypeRegistry.GetByName(MetaClassType.GetStrippedTypeName(fullType));
            // I don't check for unregistered types in old games. Due to the fact that types are unhashed, they are probably all registered. :)
        }
    }

    public override void Serialize(byte[] values, int offset, int count) => Reader.Read(values, offset, count);
}
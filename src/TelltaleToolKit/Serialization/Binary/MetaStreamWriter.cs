using System.Text;
using TelltaleToolKit.GamesDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization.Binary;

public sealed class MetaStreamWriter : MetaStream
{
    public MetaStreamWriter(Stream inputStream) : this(inputStream, MetaStreamConfiguration.Default)
    {
    }

    public MetaStreamWriter(Stream inputStream, MetaStreamConfiguration configuration)
    {
        UnderlyingStream = inputStream;

        Sections[(int)SectionType.Main].Stream = new MemoryStream();
        Sections[(int)SectionType.Debug].Stream = new MemoryStream();
        Sections[(int)SectionType.Async].Stream = new MemoryStream();
        Configuration = configuration;

        InitSerializer();
    }

    private BinaryWriter Writer { get; set; } = null!;

    public void Save()
    {
        SerializeMetaHeader();

        foreach (MetaSection section in Sections)
        {
            section.Stream.Seek(0, SeekOrigin.Begin);
            section.Stream.CopyTo(UnderlyingStream);
        }

        // Truncate, otherwise there are some leftover bytes.
        UnderlyingStream.SetLength(UnderlyingStream.Position);
    }


    public override void SerializeMetaHeader()
    {
        Writer = new BinaryWriter(UnderlyingStream);
        Writer.BaseStream.Seek(0, SeekOrigin.Begin);

        if (!Enum.IsDefined(Configuration.Version))
            throw new InvalidDataException("Version not defined");

        this.Write((uint)Configuration.Version);

        switch (Configuration.Version)
        {
            case MetaStreamVersion.Mbes:
                throw new NotSupportedException("MBES is not supported yet.");
            // TODO: optional compressed sections (set top bit of size to 1)
            case MetaStreamVersion.Msv6
                or MetaStreamVersion.Msv5:
                this.Write((int)Sections[(int)SectionType.Main].Stream.Length);
                this.Write((int)Sections[(int)SectionType.Debug].Stream.Length);
                this.Write((int)Sections[(int)SectionType.Async].Stream.Length);
                break;
        }

        Configuration.SerializedClasses = [.. Configuration.SerializedClasses.Distinct()];

        this.Write(Configuration.SerializedClasses.Count);

        foreach (MetaClass t in Configuration.SerializedClasses)
        {
            if (Configuration.AreSymbolsHashed)
            {
                this.Write(t.ClassType.Symbol.Crc64);
            }
            else
            {
                this.Write(t.ClassType.FullTypeName);
            }

            this.Write(t.Crc32);
        }
    }

    protected override void InitSerializer() => Writer = new BinaryWriter(CurrentSubstream, Encoding.UTF8, true);

    public override void BeginBlock()
    {
        GetCurrentSection().Blocks.Push(GetCurrentSection().Stream.Position);
        this.Write(0);
    }

    public override void EndBlock()
    {
        long position = GetCurrentSection().Blocks.Pop();

        var blockSize = (int)(GetCurrentSection().Stream.Position - position);
        long currentPosition = GetCurrentSection().Stream.Position;

        GetCurrentSection().Stream.Seek(position, SeekOrigin.Begin);
        this.Write(blockSize);
        GetCurrentSection().Stream.Seek(currentPosition, SeekOrigin.Begin);
    }

    public override MetaClass? GetMetaClass(Type type)
    {
        // Do not modify the metaclass descriptions. This operation is not recommended for complex types, especially those which have PropertySet (properties).
        if (!Configuration.CanModifySerializedClassesList)
        {
            return Configuration.SerializedClasses.FirstOrDefault(tc => tc.ClassType.LinkingType == type);
        }

        return TTKContext.Instance().GetMetaClassDescriptionFromActiveGame(type);
    }

    public override MetaClass? GetMetaClass(Symbol symbol)
    {
        // Do not modify the metaclass descriptions. This operation is not recommended for complex types, especially those which have PropertySet (properties).
        if (!Configuration.CanModifySerializedClassesList)
        {
            return Configuration.SerializedClasses.FirstOrDefault(tc => tc.ClassType.Symbol.Crc64 == symbol.Crc64);
        }

        return TTKContext.Instance().GetMetaClassDescriptionFromActiveGame(symbol);
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

    public override void Serialize(ref MetaClassType value)
    {
        throw new NotImplementedException();
    }

    public override void Serialize(byte[] values, int offset, int count) => Writer.Write(values, offset, count);

    public override void Serialize(ref Symbol value)
    {
        if (Configuration.AreSymbolsHashed)
        {
            MetaClass? description = GetMetaClass(typeof(Symbol));
            AddVersionInfo(description);
            Writer.Write(value.Crc64);
        }
        else
        {
            string valueSymbolName = value.SymbolName;
            Serialize(ref valueSymbolName);
        }
    }

    public void AddVersionInfo(MetaClass? desc)
    {
        if (desc is null || Configuration.SerializedClasses.Contains(desc))
        {
            return;
        }

        Configuration.SerializedClasses.Add(desc);
    }
}
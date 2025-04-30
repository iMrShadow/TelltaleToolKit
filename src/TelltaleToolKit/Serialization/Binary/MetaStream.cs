using TelltaleToolKit.HashDatabase;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization.Binary;

/// <summary>
/// Stream wrapper used for serializing telltale assets.
/// A single Telltale Tool file has 1-3 stream sections (Main, Debug, Async) depending on its meta version.
/// 
/// </summary>
public abstract class MetaStream : IDisposable
{
    private SectionType _currentSection = SectionType.Main;

    protected enum SectionType
    {
        Main,
        Debug,
        Async
    }

    protected MetaSection[] Sections =
    [
        new(), // Main section
        new(), // Debug section
        new(), // Async section
    ];

    protected internal MetaStreamConfiguration Configuration { get; set; } = new();
    
    
    // public MetaStreamVersion Version { get; set; }
    //
    // public List<MetaClass> SerializedClasses { get; set; } = [];
    // public List<(MetaClassType, uint crc32)> UnregisteredClasses { get; set; } = [];
    // public List<(ulong, uint)> UnregisteredTypes { get; set; } = [];

    protected Stream UnderlyingStream { get; init; } = null!;

    // /// <summary>
    // /// Whether the symbols are hashed.
    // /// </summary>
    // public bool AreSymbolsHashed { get; set; }

    // /// <summary>
    // /// 
    // /// </summary>
    // public List<Symbol> SerializedSymbols { get; private set; } = [];
    
    protected Stream CurrentSubstream => GetCurrentSection().Stream;

    public void Dispose()
    {
        foreach (MetaSection section in Sections)
        {
            section.Stream.Dispose();
        }

        UnderlyingStream.Dispose();
        GC.SuppressFinalize(this);
    }

    protected MetaSection GetCurrentSection() => Sections[(int)_currentSection];

    public void BeginAsyncSection()
    {
        if (_currentSection is SectionType.Async || Configuration.Version is not (MetaStreamVersion.Msv5 or MetaStreamVersion.Msv6))
            return;
        _currentSection = SectionType.Async;
        InitSerializer();
    }

    public void EndAsyncSection()
    {
        if (_currentSection is not SectionType.Async ||
            Configuration.Version is not (MetaStreamVersion.Msv5 or MetaStreamVersion.Msv6))
            return;
        _currentSection = SectionType.Main; // Switch back to header section
        InitSerializer();
    }

    public void BeginDebugSection()
    {
        if (_currentSection is SectionType.Debug ||
            Configuration.Version is not (MetaStreamVersion.Msv4 or MetaStreamVersion.Msv5 or MetaStreamVersion.Msv6))
            return;
        _currentSection = SectionType.Debug;
        InitSerializer();
    }

    public void EndDebugSection()
    {
        if (_currentSection is not SectionType.Debug ||
           Configuration.Version is not (MetaStreamVersion.Msv4 or MetaStreamVersion.Msv5 or MetaStreamVersion.Msv6))
            return;
        _currentSection = SectionType.Main;
        InitSerializer();
    }

    public abstract void SerializeMetaHeader();

    protected abstract void InitSerializer();

    public abstract void BeginBlock();

    public abstract void EndBlock();

    public bool IsSectionEmpty()
        => GetCurrentSection().Stream.Length == 0;

    public bool IsClassSerialized(string typeName)
        => Configuration.SerializedClasses.Any(id => id.ClassType.Symbol.SymbolName == typeName);

    public abstract MetaClass? GetMetaClass(Type type);

    public abstract MetaClass? GetMetaClass(Symbol symbol);

    public void Serialize<T>(ref T obj) where T : new()
    {
        MetaClassSerializer<T> serializer = TTKContext.Instance().GetSerializer<T>();
        serializer.PreSerialize(ref obj, this, null);
        serializer.Serialize(ref obj, this);
    }

    public void Serialize(ref object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        MetaClassSerializer serializer = TTKContext.Instance().GetSerializer(obj.GetType());
        serializer.PreSerialize(ref obj, this);
        serializer.Serialize(ref obj, this);
    }

    /// <summary>
    /// Serializes the specified boolean value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref bool value);

    /// <summary>
    /// Serializes the specified float value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref float value);

    /// <summary>
    /// Serializes the specified double value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref double value);

    /// <summary>
    /// Serializes the specified short value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref short value);

    /// <summary>
    /// Serializes the specified integer value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref int value);

    /// <summary>
    /// Serializes the specified long value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref long value);

    /// <summary>
    /// Serializes the specified ushort value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref ushort value);

    /// <summary>
    /// Serializes the specified unsigned integer value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref uint value);

    /// <summary>
    /// Serializes the specified unsigned long value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref ulong value);

    /// <summary>
    /// Serializes the specified string value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref string value);

    /// <summary>
    /// Serializes the specified char value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref char value);

    /// <summary>
    /// Serializes the specified byte value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref byte value);

    /// <summary>
    /// Serializes the specified signed byte value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref sbyte value);

    /// <summary>
    /// Serializes the Symbol class.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref Symbol value);

    /// <summary>
    /// Serializes the MetaClassType class. A special reader/writer for types.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref MetaClassType value);

    /// <summary>
    /// Serializes the specified byte array.
    /// </summary>
    /// <param name="values">The buffer to serialize.</param>
    /// <param name="offset">The starting offset in the buffer to begin serializing.</param>
    /// <param name="count">The size, in bytes, to serialize.</param>
    public abstract void Serialize(byte[] values, int offset, int count);

    public void ThrowIfNotEndOfFile()
    {
        foreach (MetaSection stream in Sections)
        {
            var reader = new StreamReader(stream.Stream);
            if (reader.BaseStream.Position != stream.Stream.Length)
            {
                throw new InvalidDataException(
                    $"FILE END NOT REACHED! Current position: {reader.BaseStream.Position}. Total size: {reader.BaseStream.Length}. Left to read: {reader.BaseStream.Length - reader.BaseStream.Position}");
            }
        }
    }

    // If there are assets larger than 2GBs, I need to rethink my life.
    public int GetRemainingSectionBytes()
        => (int)(GetCurrentSection().Stream.Length - GetCurrentSection().Stream.Position);

    public long GetCurrentPosition()
        => GetCurrentSection().Stream.Position;

    public void SetCurrentPosition(long position)
        => GetCurrentSection().Stream.Position = position;

    public void ResolveSymbols(ISymbolResolver symbolResolver)
    {
        foreach (Symbol entry in Configuration.SerializedSymbols)
        {
            symbolResolver.ResolveSymbol(entry);

            if (!entry.HasString())
            {
                Console.WriteLine($"Could not resolve symbol for hash: {entry.Crc64}");
            }
        }
    }

    public class MetaSection
    {
        // For blocks. In read, stores the sizes, in write stores the block offset initial.
        public Stack<long> Blocks = [];

        public bool IsCompressed = false;

        // Section data stream
        public Stream Stream = null!;
    };
}
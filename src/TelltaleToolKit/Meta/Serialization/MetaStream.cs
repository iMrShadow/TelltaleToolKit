using System.Text;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization.Binary;
using TelltaleToolKit.Meta.Serialization.Json;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Meta.Serialization;

/// <summary>
///     Stream wrapper used for serializing telltale assets.
///     A single Telltale Tool file has 1-4 stream sections (Header, Default, Debug, Async) depending on its metastream
///     version.
/// </summary>
public abstract class MetaStream : IDisposable
{
    public enum SectionType
    {
        Header,
        Default,
        Debug,
        Async
    }

    protected readonly SectionInfo[] Sections =
    [
        new(), // Header section
        new(), // Default section
        new(), // Debug section
        new() // Async section
    ];

    /// <summary>Gets the currently active section (Header, Default, Debug, Async).</summary>
    protected SectionType _currentSection = SectionType.Header;

    public MetaStreamParams Params { get; set; } = new();

    protected Stream BaseStream { get; set; } = null!;

    protected int DebugSectionDepth = 0;

    public int DefaultSectionDepth = 0;

    /// <summary>
    ///     Gets the mode of this stream (read or write).
    /// </summary>
    public abstract MetaStreamMode Mode { get; }

    /// <summary>Gets the currently active section.</summary>
    protected SectionInfo CurrentSection => Sections[(int)_currentSection];

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Opens a MetaStream for reading from the specified input stream.
    /// </summary>
    /// <param name="inputStream">The stream containing the Telltale asset.</param>
    /// <param name="workspace">Optional workspace required for legacy decryption.</param>
    /// <returns>A MetaStream configured for reading.</returns>
    public static MetaStream OpenRead(Stream inputStream, Workspace? workspace = null)
        => new BinaryMetaStreamReader(inputStream, workspace);

    /// <summary>
    ///     Opens a MetaStream for writing to the specified output stream.
    /// </summary>
    /// <param name="outputStream">The stream that will receive the serialized data.</param>
    /// <param name="configuration">Parameters that define the version, registered classes, etc.</param>
    /// <returns>A MetaStream configured for writing.</returns>
    public static MetaStream OpenWrite(Stream outputStream, MetaStreamParams configuration)
        => new BinaryMetaStreamWriter(outputStream, configuration);

    /// <summary>
    ///     Opens a MetaStream for reading from the specified input stream.
    /// </summary>
    /// <param name="inputStream">The stream containing the Telltale asset.</param>
    /// <param name="configuration">Parameters that define the version, registered classes, etc.</param>
    /// <returns>A MetaStream configured for writing.</returns>
    public static MetaStream OpenJsonRead(Stream inputStream, MetaStreamParams configuration)
        => new JsonMetaStreamReader(inputStream, configuration);

    /// <summary>
    ///     Opens a MetaStream for writing to the specified output stream.
    /// </summary>
    /// <param name="outputStream">The stream that will receive the serialized data.</param>
    /// <param name="configuration">Parameters that define the version, registered classes, etc.</param>
    /// <returns>A MetaStream configured for writing.</returns>
    public static MetaStream OpenJsonWrite(Stream outputStream, MetaStreamParams configuration)
        => new JsonMetaStreamWriter(outputStream, configuration);


    public virtual bool BeginAsyncSection()
    {
        return _currentSection == SectionType.Default && SetSection(SectionType.Async);
    }

    public virtual void EndAsyncSection()
    {
        if (_currentSection == SectionType.Async)
        {
            SetSection(SectionType.Default);
        }
    }

    public virtual bool BeginDebugSection()
    {
        if ((_currentSection is SectionType.Default && DebugSectionDepth == 0 && SetSection(SectionType.Debug))
            || (_currentSection == SectionType.Debug && DebugSectionDepth >= 0))
        {
            if (Mode is MetaStreamMode.Read && Sections[2].Stream?.Length == 0)
            {
                SetSection(SectionType.Default);
                return false;
            }

            DebugSectionDepth++;
            return true;
        }

        return false;
    }

    public virtual void EndDebugSection()
    {
        if (_currentSection != SectionType.Debug || DebugSectionDepth <= 0)
        {
            return;
        }

        DebugSectionDepth--;
        if (DebugSectionDepth == 0)
        {
            SetSection(SectionType.Default);
        }
    }

    /// <summary>
    ///     Emits a property key for the next value. No-op in binary streams;
    ///     in JSON streams this labels the next value within an object container.
    /// </summary>
    /// <param name="name">The property/field name.</param>
    public virtual void Key(string name)
    {
    }

    /// <summary>
    ///     Begins a named object or array container.
    ///     No-op in binary streams. In JSON streams emits the opening brace or bracket.
    /// </summary>
    public virtual void BeginObject(string name, bool isArray = false)
    {
    }

    /// <summary>
    ///     Ends the most recently opened named container.
    ///     No-op in binary streams. In JSON streams emits the closing brace or bracket.
    /// </summary>
    public virtual void EndObject(string name)
    {
    }

    /// <summary>
    ///     Called when the current section changes. Implementations should set up their reader/writer
    ///     to point to the new section's stream.
    /// </summary>
    /// <param name="section">The section that is now active.</param>
    protected abstract bool SetSection(SectionType section);

    public abstract void BeginBlock();

    public abstract void EndBlock();

    public bool IsSectionEmpty()
        => Sections[(int)_currentSection].Stream?.Length == 0;

    public bool IsClassSerialized(string typeName)
        => Params.GetRegisteredClasses().Any(id => id.ClassType.Symbol.DebugString == typeName);

    public MetaClass? GetMetaClass(Type type)
    {
        if (!Params.CanModifySerializedClassesList)
        {
            var versionInfo = Params.VersionInfo
                .FirstOrDefault(versionInfo => versionInfo.GetMetaClassType()?.LinkingType == type);

            if (versionInfo == null)
                Toolkit.Instance.Logger.LogError($"MetaClass not found for type: {type}");

            var metaClass = versionInfo?.GetMetaClass();

            if (metaClass == null)
                Toolkit.Instance.Logger.LogError($"MetaClass not registered for type: {type}");
            return metaClass;
        }

        return Params.Workspace?.GetMetaClassDescription(type);
    }

    public MetaClass? GetMetaClass(Symbol symbol)
    {
        if (!Params.CanModifySerializedClassesList)
        {
            return Params.VersionInfo.FirstOrDefault(versionInfo => versionInfo.TypeSymbolCrc == symbol.Crc64)
                ?.GetMetaClass();
        }

        return Params.Workspace?.GetMetaClassDescription(symbol);
    }

    /// <summary>
    ///     Serializes the specified boolean value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref bool value);

    /// <summary>
    ///     Serializes the specified float value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref float value);

    /// <summary>
    ///     Serializes the specified double value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref double value);

    /// <summary>
    ///     Serializes the specified short value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref short value);

    /// <summary>
    ///     Serializes the specified integer value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref int value);

    /// <summary>
    ///     Serializes the specified long value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref long value);

    /// <summary>
    ///     Serializes the specified ushort value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref ushort value);

    /// <summary>
    ///     Serializes the specified unsigned integer value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref uint value);

    /// <summary>
    ///     Serializes the specified unsigned long value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref ulong value);

    /// <summary>
    ///     Serializes the specified string value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref string value);

    /// <summary>
    ///     Serializes the specified char value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref char value);

    /// <summary>
    ///     Serializes the specified byte value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref byte value);

    /// <summary>
    ///     Serializes the specified signed byte value.
    /// </summary>
    /// <param name="value">The value to serialize</param>
    public abstract void Serialize(ref sbyte value);

    /// <summary>
    ///     Serializes the specified byte array.
    /// </summary>
    /// <param name="values">The buffer to serialize.</param>
    /// <param name="offset">The starting offset in the buffer to begin serializing.</param>
    /// <param name="count">The size, in bytes, to serialize.</param>
    public abstract void Serialize(byte[] values, int offset, int count);

    public bool IsEndOfStream()
    {
        if (Mode is not MetaStreamMode.Read)
        {
            return false;
        }

        for (int i = 1; i <= 3; i += 2)
        {
            //for each section (default,debug,async)
            SectionInfo currentSect = Sections[i];
            if (currentSect.Stream == null)
            {
                continue;
            }

            if (currentSect.Stream.Position != currentSect.Stream.Length)
            {
                if (i == 2)
                {
                    Toolkit.Instance.Logger.LogWarning(@"Unexpected end of debug section (non-critical). Position: " +
                                                       currentSect.Stream.Position +
                                                       @" Length: " + currentSect.Stream.Length);
                }
                else
                {
                    Toolkit.Instance.Logger.LogError(@"Unexpected end of stream. Position: " +
                                                     currentSect.Stream.Position +
                                                     @" Length: " + currentSect.Stream.Length);
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    ///     Skip a block.
    ///     This is internally used, even by Telltale themselves.
    /// </summary>
    public void SkipToEndOfCurrentBlock()
    {
        if (Mode is not MetaStreamMode.Read)
        {
            return;
        }

        SectionInfo currentSectionInfo = Sections[(int)_currentSection];
        if (currentSectionInfo.Blocks.Count == 0)
        {
            return;
        }

        long expectedPosition = currentSectionInfo.Blocks.Pop();
        currentSectionInfo.Stream?.Seek(expectedPosition, SeekOrigin.Begin);
    }

    /// <summary>Gets the underlying stream for the specified section.</summary>
    /// <param name="type">The section type (Header, Default, Debug, Async).</param>
    /// <returns>The stream if the section exists and is initialized; otherwise null.</returns>
    public SectionInfo GetSection(SectionType type)
        => Sections[(int)type];

    // If there are assets larger than 2GBs, I need to rethink my life.
    public long GetRemainingSectionBytes()
    {
        Stream? stream = CurrentSection.Stream;
        if (stream == null)
        {
            return 0;
        }

        return stream.Length - stream.Position;
    }

    public long GetRemainingSectionBytes(SectionType section)
    {
        Stream? stream = Sections[(int)section].Stream;
        if (stream == null)
        {
            return 0;
        }

        return stream.Length - stream.Position;
    }

    public long GetPosition()
    {
        Stream? stream = CurrentSection.Stream;
        if (stream == null)
        {
            return 0;
        }

        return stream.Position;
    }

    public void SetPosition(long position)
    {
        Stream? stream = CurrentSection.Stream;
        if (stream == null)
        {
            throw new InvalidOperationException("Cannot set position on a null section stream.");
        }

        stream.Position = position;
    }

    public void AddVersionInfo(MetaClass? desc)
    {
        if (desc is null)
        {
            return;
        }

        foreach (MetaVersionInfo? versInfo in Params.VersionInfo)
        {
            if (versInfo.TypeSymbolCrc == desc.ClassType.Symbol.Crc64)
            {
                if (versInfo.VersionCrc != desc.Crc32)
                {
                    throw new InvalidOperationException("Version CRC mismatch");
                }

                return;
            }
        }

        MetaVersionInfo versionInfo = new() { TypeSymbolCrc = desc.ClassType.Symbol.Crc64, VersionCrc = desc.Crc32 };
        Params.VersionInfo.Add(versionInfo);
    }

    /// <summary>
    ///     Closes the stream and finalizes writing (if in write mode).
    /// </summary>
    public abstract void Close();

    public static bool IsValidMetaStream(Stream stream)
    {
        if (stream.Length < 4)
        {
            return false;
        }

        long originalPosition = stream.Position;

        try
        {
            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            uint version = reader.ReadUInt32();
            return Enum.IsDefined(typeof(MetaStreamMagic), version);
        }
        finally
        {
            stream.Position = originalPosition;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (SectionInfo section in Sections)
            {
                section.Stream?.Dispose();
            }
        }
    }

    public class SectionInfo
    {
        // For blocks. In read, stores the sizes, in write stores the block offset initial.
        public readonly Stack<long> Blocks = [];

        public long CompressedSize = 0;
        public bool IsCompressed = false;

        public bool IsEnabled = true;

        // Section data stream
        public Stream? Stream = null;
    }
}

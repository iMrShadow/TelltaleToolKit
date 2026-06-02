using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.IO.Archives;

/// <summary>
///     Represents a single file record inside a Telltale archive.
/// </summary>
/// <remarks>
///     <para>
///         <see cref="NameCrc" /> is the canonical lookup key used by <see cref="Archive" />'s
///         internal dictionary. It is computed from <see cref="Name" /> when the archive is loaded
///         and must not change after insertion.
///     </para>
/// </remarks>
[MetaSerializer(typeof(MetaClassSerializer<ResourceEntry>))]
public sealed class ResourceEntry
{
    /// <summary>
    ///     Gets the CRC-64 hash of <see cref="Name" />. This is the dictionary key in <see cref="Archive" />.
    /// </summary>
    [MetaMember("mNameCrc")]
    public ulong NameCrc { get; init; }

    /// <summary>
    ///     Gets or sets the original filename stored in the archive.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the byte offset of this entry's data relative to the start of the
    ///     file-data region (<c>ArchiveInfo.FilesOffset</c>).
    /// </summary>
    /// <remarks>
    ///     Internal - mutated by archive writers when packing or repacking.
    /// </remarks>
    [MetaMember("mOffset")]
    public ulong Offset { get; internal init; }

    /// <summary>
    ///     Gets or sets the raw (compressed) byte size of this entry's data in the archive.
    /// </summary>
    /// <remarks>
    ///     Internal - mutated by archive writers when packing or repacking.
    /// </remarks>
    [MetaMember("mSize")]
    public uint Size { get; internal init; }

    /// <summary>
    ///     No idea. Needs investigation.
    /// </summary>
    /// <remarks>
    ///     Internal - mutated by archive writers when packing or repacking.
    /// </remarks>
    [MetaMember("mPreloadSize")]
    public uint PreloadSize { get; internal set; }

    /// <summary>
    ///     Gets or sets the index of the name page containing this entry's filename.
    /// </summary>
    /// <remarks>
    ///     Internal - mutated by archive writers when packing or repacking.
    /// </remarks>
    [MetaMember("mNamePageIndex")]
    public ushort NamePageIndex { get; internal init; }

    /// <summary>
    ///     Gets or sets the offset of this entry's filename within the name page.
    /// </summary>
    /// <remarks>
    ///     Internal - mutated by archive writers when packing or repacking.
    /// </remarks>
    [MetaMember("mNamePageOffset")]
    public ushort NamePageOffset { get; internal init; }

    /// <inheritdoc />
    public override string ToString() => $"{Name} (CRC: {NameCrc:X16}, Offset: {Offset}, Size: {Size})";
}

using TelltaleToolKit.Serialization;
using TelltaleToolKit.Utility.Compression;

namespace TelltaleToolKit.TelltaleArchives;

/// <summary>
///     Controls how an archive is written or repacked.
/// </summary>
public sealed class ArchiveWriteOptions
{
    /// <summary>
    ///     Gets or sets the inner archive version to embed in the payload header.
    ///     Defaults to <see cref="TTArchiveVersion.Tta4" />.
    /// </summary>
    public TTArchiveVersion TTArchiveVersion { get; set; } = TTArchiveVersion.Tta4;

    /// <summary>
    ///     Gets or sets whether Blowfish encryption is applied to each chunk.
    ///     <para>
    ///         When <see langword="true" />, <see cref="BlowfishKey" /> must be non-empty.
    ///         Ignored for container versions that encode encryption in the version magic
    ///         (e.g. <see cref="ContainerMagic.TTCE" />); the magic takes precedence.
    ///     </para>
    ///     Defaults to <see langword="false" />.
    /// </summary>
    public bool Encrypt { get; set; }

    /// <summary>
    ///     Gets or sets the compression algorithm to use.
    ///     Defaults to <see cref="Compression.Deflate" />.
    /// </summary>
    public Compression Algorithm { get; set; } = Compression.Deflate;

    /// <summary>
    ///     Gets or sets the decompressed size of each chunk in bytes.
    ///     The value is rounded up to the nearest multiple of 1 024 when writing
    ///     <c>.ttarch</c> (version ≥ 7) archives.
    ///     Defaults to 65 536 (64 KiB).
    /// </summary>
    public uint ChunkSize { get; set; } = 65_536;

    /// <summary>
    ///     Gets or sets a value indicating whether to set the XMode.
    ///     Set this to <see langword="true" /> if the archive crashes when loading in the game.
    /// </summary>
    public bool XMode { get; set; } = false;

    /// <summary>
    ///     Gets or sets the Blowfish key used when <see cref="Encrypt" /> is
    ///     <see langword="true" />.
    ///     Ignored when the archive is not encrypted.
    /// </summary>
    public string BlowfishKey { get; set; } = string.Empty;
}



namespace TelltaleToolKit.IO.Archives;

/// <summary>
///     Contains metadata about a Telltale archive, including its container type,
///     version, flags, chunk layout, encryption key, and file data offset.
/// </summary>
/// <remarks>
///     This information is populated during archive loading and used by
///     <see cref="Archive" /> subclasses to correctly read entries and data.
/// </remarks>
public class ArchiveInfo
{
    /// <summary>
    ///     Gets or sets the inner archive version (Legacy, 1-9, TTA2, TTA3, TTA4).
    /// </summary>
    public TTArchiveVersion Version { get; set; }

    /// <summary>
    ///     Gets or sets flags indicating encryption, compression, and other properties.
    /// </summary>
    public ArchiveFlags Flags { get; set; }

    /// <summary>
    ///     Gets or sets the decompressed size of each chunk in bytes (0 if uncompressed).
    /// </summary>
    public uint ChunkSize { get; set; }

    /// <summary>
    ///     Gets or sets the number of compressed chunks (0 if uncompressed).
    /// </summary>
    public uint ChunkCount { get; set; }

    /// <summary>
    ///     Gets or sets an array of compressed sizes for each chunk.
    ///     Length equals <see cref="ChunkCount" />; empty if uncompressed.
    /// </summary>
    public ulong[] ChunkBlockSizes { get; set; } = [];

    /// <summary>
    ///     Gets or sets the Blowfish key used for encryption/decryption.
    ///     Must match the key used by the original game.
    /// </summary>
    public string BlowfishKey { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the byte offset (from the start of the archive stream) where the raw file data region begins.
    /// </summary>
    public ulong FilesOffset { get; set; }
}

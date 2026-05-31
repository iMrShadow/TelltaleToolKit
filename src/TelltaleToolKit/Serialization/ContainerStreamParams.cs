using TelltaleToolKit.Utility.Compression;

namespace TelltaleToolKit.Serialization;

/// <summary>
/// Parameters for creating a Telltale Tool Container stream.
/// Mirrors the engine's <c>DataStreamContainerParams</c>.
/// </summary>
public sealed class ContainerStreamParams
{
    /// <summary>Decompressed size of each chunk. Defaults to 0x10000 (64 KB).</summary>
    public uint ChunkSize { get; init; } = 0x10000;

    /// <summary>Whether to encrypt compressed chunks with Blowfish.</summary>
    public bool Encrypt { get; init; }

    /// <summary>
    /// The compression algorithm used to compress the container.
    /// Defaults to Deflate (ZLIB in Telltale's terms).
    /// </summary>
    public Compression Algorithm { get; init; } = Compression.Deflate;

    /// <summary>
    /// Blowfish key used when <see cref="Encrypt"/> is true.
    /// Kept separate from pure format parameters — pass <c>string.Empty</c> when not encrypting.
    /// </summary>
    public string BlowfishKey { get; init; } = string.Empty;
}

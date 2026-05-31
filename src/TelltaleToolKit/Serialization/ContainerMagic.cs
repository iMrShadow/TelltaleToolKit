namespace TelltaleToolKit.Serialization;

/// <summary>
///     Four‑byte magic numbers identifying the container format of a file.
/// </summary>
/// <remarks>
///     The magic determines whether the archive is compressed, encrypted,
///     and which compression algorithm (zlib or Oodle) is used.
/// </remarks>
public enum ContainerMagic
{
    /// <summary>
    ///     "TTCN" – No compression, no encryption. Plain container.
    /// </summary>
    TTCN = 0x5454434E,

    /// <summary>
    ///     "TTCE" – Encrypted container with raw DEFLATE compression.
    /// </summary>
    TTCE = 0x54544345,

    /// <summary>
    ///     "TTCe" – Encrypted container supporting Oodle compression.
    /// </summary>
    TTCe = 0x54544365,

    /// <summary>
    ///     "TTCZ" – Zlib‑compressed container (raw DEFLATE), no encryption.
    /// </summary>
    TTCZ = 0x5454435A,

    /// <summary>
    ///     "TTCz" – Zlib‑compressed container (raw DEFLATE) supporting Oodle compression.
    /// </summary>
    TTCz = 0x5454437A
}

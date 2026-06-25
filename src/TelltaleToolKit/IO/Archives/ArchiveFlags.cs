using System;

namespace TelltaleToolKit.IO.Archives;

/// <summary>
///     Flags that describe the encryption and compression state of a Telltale archive.
/// </summary>
[Flags]
public enum ArchiveFlags
{
    /// <summary>No special flags.</summary>
    None = 0,

    /// <summary>Archive is encrypted with Blowfish (standard variant).</summary>
    IsEncrypted = 1 << 1,

    /// <summary>Archive is compressed using zlib (wrapped DEFLATE).</summary>
    IsZlibCompressed = 1 << 3,

    /// <summary>Archive is compressed using raw DEFLATE (no zlib header).</summary>
    IsRawDeflateCompressed = 1 << 4,

    /// <summary>Archive is compressed using Oodle (not yet supported).</summary>
    IsOodleCompressed = 1 << 5,

    /// <summary>Archive uses a modified Blowfish variant. That's all .ttarch2 and version >= 7 .ttarch files.</summary>
    IsModifiedBlowfishEncrypted = 1 << 6,

    /// <summary>Archive uses "XMode" – an unknown flag seen in some version >= 5 archives.</summary>
    IsXMode = 1 << 7
}

/// <summary>
///     Extension methods for <see cref="ArchiveFlags" /> to simplify common checks.
/// </summary>
public static class ArchiveFlagsExtensions
{
    /// <summary>
    ///     Indicates whether the archive uses any form of compression.
    /// </summary>
    /// <param name="value">The flags value.</param>
    /// <returns>True if compressed (raw DEFLATE, zlib, or Oodle).</returns>
    public static bool IsCompressed(this ArchiveFlags value)
        => value.HasFlag(ArchiveFlags.IsRawDeflateCompressed) || value.HasFlag(ArchiveFlags.IsZlibCompressed) ||
           value.HasFlag(ArchiveFlags.IsOodleCompressed);

    /// <summary>
    ///     Indicates whether the archive is encrypted (standard Blowfish).
    /// </summary>
    /// <param name="value">The flags value.</param>
    /// <returns>True if encrypted.</returns>
    /// <remarks>Does not detect <see cref="ArchiveFlags.IsModifiedBlowfishEncrypted" />.</remarks>
    public static bool IsEncrypted(this ArchiveFlags value)
        => value.HasFlag(ArchiveFlags.IsEncrypted) || value.HasFlag(ArchiveFlags.IsModifiedBlowfishEncrypted);
}

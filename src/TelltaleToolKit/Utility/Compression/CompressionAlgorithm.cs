namespace TelltaleToolKit.Utility.Compression;

/// <summary>Compression algorithm to use when writing archive chunks.</summary>
public enum CompressionAlgorithm
{
    /// <summary>No compression.</summary>
    None,

    /// <summary>Raw DEFLATE (no zlib header). Used by most Telltale games.</summary>
    Deflate,

    /// <summary>Zlib-wrapped DEFLATE. Used by some CSI and earlier titles.</summary>
    Zlib,

    /// <summary>
    ///     Oodle compression.
    /// </summary>
    Oodle
}

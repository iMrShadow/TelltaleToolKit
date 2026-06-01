namespace TelltaleToolKit.Serialization;

/// <summary>
///     Four‑character codes (magic numbers) that identify the format and encryption state
///     of a Telltale MetaStream file.
/// </summary>
/// <remarks>
///     The magic appears as the first 4 bytes of the file. Plaintext magics are human‑readable
///     ASCII strings (e.g. "NIBM" in little-Endian). Encrypted magics are arbitrary 32‑bit values chosen by
///     Telltale to avoid easy pattern matching.
/// </remarks>
public enum MetaStreamMagic : uint
{
    /// <summary>
    ///     MetaStream version 1.<br />
    ///     Expansion: "Meta Binary".<br />
    ///     ASCII value: "MBIN" in big-Endian, or "NIBM" in little-Endian.<br />
    ///     Earliest Telltale asset version.<br />
    ///     Version info is plain text..
    /// </summary>
    Mbin = 0x4D42494E,

    /// <summary>
    ///     MetaStream version 1 (encrypted).<br />
    ///     Expansion: "Meta Binary Encrypted Stream".<br />
    ///     ASCII value: "MBES" in big-Endian, or "SEBM" in little-Endian.<br />
    ///     Used in some early encrypted assets.<br />
    ///     Version info is plain text after decryption.
    /// </summary>
    Mbes = 0x4D424553,

    /// <summary>
    ///     MetaStream version 2 (encrypted, variant 1).<br />
    ///     Telltale used three different arbitrary values for version 2 assets.<br />
    ///     These are determined by a number generator.<br />
    ///     Version info is plain text after decryption.
    /// </summary>
    EncryptedMbin1 = 0xFB4A1764,

    /// <summary>
    ///     MetaStream version 2 (encrypted, variant 2).<br />
    ///     Telltale used three different arbitrary values for version 2 assets.<br />
    ///     These are determined by a number generator.<br />
    ///     Version info is plain text after decryption.
    /// </summary>
    EncryptedMbin2 = 0xEB794091,

    /// <summary>
    ///     MetaStream version 2 (encrypted, variant 3).<br />
    ///     Telltale used three different arbitrary values for version 2 assets.<br />
    ///     These are determined by a number generator.<br />
    ///     Version info is plain text after decryption.
    /// </summary>
    EncryptedMbin3 = 0x64AFDEFB,

    /// <summary>
    ///     MetaStream version 3.<br />
    ///     Expansion: "Meta T(h)ree" or "Meta Type Reference."<br />
    ///     ASCII value: "MTRE" in big-Endian, or "ERTM" in little-Endian.<br />
    ///     Version info is hashed.
    /// </summary>
    Mtre = 0x4D545245,

    /// <summary>
    ///     MetaStream version 3 (encrypted).<br />
    ///     Version info is hashed after decryption.
    /// </summary>
    EncryptedMtre = 0x64AFDEAA,

    /// <summary>
    ///     MetaStream version 3 (compressed).<br />
    ///     Expansion: "Meta Compressed".<br />
    ///     ASCII value: "MCOM" in big-Endian, or "MOCM" in little-Endian.<br />
    ///     Version info is hashed after decompression.
    /// </summary>
    Mcom = 0x4D434F4D,

    /// <summary>
    ///     MetaStream version 3 (compressed, encrypted).<br />
    ///     Version info is hashed after decompression and decryption (presumably).
    /// </summary>
    EncryptedMcom = 0x64AFDEBB,

    /// <summary>
    ///     MetaStream version 4 (optional compression).<br />
    ///     Expansion: "Meta Stream Version 4".<br />
    ///     ASCII value: "MSV4" in big-Endian, or "4VSM" in little-Endian.<br />
    ///     Version info is hashed.<br />
    ///     Unused in any games.
    /// </summary>
    Msv4 = 0x4D535356,

    /// <summary>
    ///     MetaStream version 5 (optional compression).<br />
    ///     Expansion: "Meta Stream Version 5".<br />
    ///     ASCII value: "MSV5" in big-Endian, or "5VSM" in little-Endian.<br />
    ///     Version info is hashed.
    /// </summary>
    Msv5 = 0x4D535635,

    /// <summary>
    ///     MetaStream version 6 (optional compression).<br />
    ///     Expansion: "Meta Stream Version 6".<br />
    ///     ASCII value: "MSV6" in big-Endian, or "6VSM" in little-Endian.<br />
    ///     Version info is hashed.
    /// </summary>
    Msv6 = 0x4D535636
}

/// <summary>
///     Extension methods for <see cref="MetaStreamMagic" />.
/// </summary>
public static class MetaStreamVersionExtensions
{
    /// <summary>
    ///     Returns the MetaStream format version associated with the given magic number.
    /// </summary>
    /// <param name="magic">The magic value read from the file header.</param>
    /// <returns>
    ///     An integer representing the version of the MetaStream format:
    ///     <list type="table">
    ///         <item>
    ///             <term>1</term><description>Plaintext (Mbin, Mbes) or encrypted Mbes</description>
    ///         </item>
    ///         <item>
    ///             <term>2</term><description>Encrypted version 2 (EncryptedMbin1/2/3)</description>
    ///         </item>
    ///         <item>
    ///             <term>3</term><description>Version 3 (Mtre, Mcom, EncryptedMtre, EncryptedMcom)</description>
    ///         </item>
    ///         <item>
    ///             <term>4</term><description>Version 4 (Msv4)</description>
    ///         </item>
    ///         <item>
    ///             <term>5</term><description>Version 5 (Msv5)</description>
    ///         </item>
    ///         <item>
    ///             <term>6</term><description>Version 6 (Msv6)</description>
    ///         </item>
    ///         <item>
    ///             <term>0</term><description>Unknown or unsupported magic</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <remarks>
    ///     The returned version corresponds to the <see cref="MetaStreamParams.StreamVersion" />
    ///     that should be used when reading or writing the stream.
    /// </remarks>
    public static uint GetMetaStreamVersion(this MetaStreamMagic magic) =>
        magic switch
        {
            MetaStreamMagic.Mbin or MetaStreamMagic.Mbes => 1,
            MetaStreamMagic.EncryptedMbin1 or MetaStreamMagic.EncryptedMbin2
                or MetaStreamMagic.EncryptedMbin3 => 2, // no basic version
            MetaStreamMagic.Mtre or MetaStreamMagic.Mcom or MetaStreamMagic.EncryptedMcom
                or MetaStreamMagic.EncryptedMtre => 3,
            MetaStreamMagic.Msv4 => 4,
            MetaStreamMagic.Msv5 => 5,
            MetaStreamMagic.Msv6 => 6,
            _ => 0
        };
}

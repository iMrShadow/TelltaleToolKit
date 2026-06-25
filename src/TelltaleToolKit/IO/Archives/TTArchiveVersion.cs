using System;

namespace TelltaleToolKit.IO.Archives;

/// <summary>
///     Version identifier for the inner Telltale archive format.
/// </summary>
/// <remarks>
///     <para>
///         Versions 0–9 are used by the original <c>.ttarch</c> format.
///         Versions <see cref="Tta2" />, <see cref="Tta3" />, <see cref="Tta4" />
///         are used by the newer <c>.ttarch2</c> format and are stored as four‑character codes.
///     </para>
/// </remarks>
public enum TTArchiveVersion
{
    /// <summary>Legacy version 0 (unversioned, used by very early games).</summary>
    Legacy = 0,

    /// <summary>Version 1 – basic header with encryption flag.</summary>
    One,

    /// <summary>Version 2 – adds an unknown uint field.</summary>
    Two,

    /// <summary>Version 3 – introduces compression and chunk tables.</summary>
    Three,

    /// <summary>Version 4 – adds two unknown uint fields (priority?).</summary>
    Four,

    /// <summary>Version 5 – adds XMode flags.</summary>
    Five,

    /// <summary>Version 6 – adds compressed entry table support.</summary>
    Six,

    /// <summary>Version 7 – chunk size stored as multiple of 1024.</summary>
    Seven,

    /// <summary>Version 8 – adds optional symbol table(?).</summary>
    Eight,

    /// <summary>Version 9 – adds header CRC32.</summary>
    Nine,

    /// <summary>
    ///     "TTA2" – <c>.ttarch2</c> format version 2 (unused).
    /// </summary>
    Tta2 = 0x54544132,

    /// <summary>
    ///     "TTA3" – <c>.ttarch2</c> format version 3.
    /// </summary>
    Tta3 = 0x54544133,

    /// <summary>
    ///     "TTA4" – <c>.ttarch2</c> format version 4.
    /// </summary>
    Tta4 = 0x54544134
}

/// <remarks>
///     Use <see cref="TTArchiveVersionExtensions.ToJsonNumber" /> and
///     <see cref="TTArchiveVersionExtensions.TtarchVersionFromNumber" />
///     to convert between the enum and integer representations.
/// </remarks>
public static class TTArchiveVersionExtensions
{
    /// <summary>
    ///     Converts an int to ArchiveVersion.
    /// </summary>
    public static TTArchiveVersion TtarchVersionFromNumber(this int value, bool isArchive2)
    {
        if (isArchive2)
        {
            return value switch
            {
                2 => TTArchiveVersion.Tta2,
                3 => TTArchiveVersion.Tta3,
                4 => TTArchiveVersion.Tta4,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }

        return value switch
        {
            0 => TTArchiveVersion.Legacy,
            1 => TTArchiveVersion.One,
            2 => TTArchiveVersion.Two,
            3 => TTArchiveVersion.Three,
            4 => TTArchiveVersion.Four,
            5 => TTArchiveVersion.Five,
            6 => TTArchiveVersion.Six,
            7 => TTArchiveVersion.Seven,
            8 => TTArchiveVersion.Eight,
            9 => TTArchiveVersion.Nine,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    /// <summary>
    ///     Converts ArchiveVersion to int.
    /// </summary>
    public static int ToJsonNumber(this TTArchiveVersion version) =>
        version switch
        {
            TTArchiveVersion.Legacy => 0,
            TTArchiveVersion.One => 1,
            TTArchiveVersion.Two => 2,
            TTArchiveVersion.Three => 3,
            TTArchiveVersion.Four => 4,
            TTArchiveVersion.Five => 5,
            TTArchiveVersion.Six => 6,
            TTArchiveVersion.Seven => 7,
            TTArchiveVersion.Eight => 8,
            TTArchiveVersion.Nine => 9,
            TTArchiveVersion.Tta2 => 2,
            TTArchiveVersion.Tta3 => 3,
            TTArchiveVersion.Tta4 => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
        };
}

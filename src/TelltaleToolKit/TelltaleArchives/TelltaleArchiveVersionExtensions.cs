namespace TelltaleToolKit.TelltaleArchives;

public static class TelltaleArchiveVersionExtensions
{
    /// <summary>
    /// Converts an int to ArchiveVersion.
    /// </summary>
    public static ArchiveVersion TtarchVersionFromNumber(this int value, bool isArchive2)
    {
        if (isArchive2)
        {
            return value switch
            {
                2 => ArchiveVersion.Tta2,
                3 => ArchiveVersion.Tta3,
                4 => ArchiveVersion.Tta4,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }

        return value switch
        {
            0 => ArchiveVersion.Zero,
            1 => ArchiveVersion.One,
            2 => ArchiveVersion.Two,
            3 => ArchiveVersion.Three,
            4 => ArchiveVersion.Four,
            5 => ArchiveVersion.Five,
            6 => ArchiveVersion.Six,
            7 => ArchiveVersion.Seven,
            8 => ArchiveVersion.Eight,
            9 => ArchiveVersion.Nine,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    /// <summary>
    /// Converts ArchiveVersion to int.
    /// </summary>
    public static int ToJsonNumber(this ArchiveVersion version)
    {
        return version switch
        {
            ArchiveVersion.Zero => 0,
            ArchiveVersion.One => 1,
            ArchiveVersion.Two => 2,
            ArchiveVersion.Three => 3,
            ArchiveVersion.Four => 4,
            ArchiveVersion.Five => 5,
            ArchiveVersion.Six => 6,
            ArchiveVersion.Seven => 7,
            ArchiveVersion.Eight => 8,
            ArchiveVersion.Nine => 9,
            ArchiveVersion.Tta2 => 2,
            ArchiveVersion.Tta3 => 3,
            ArchiveVersion.Tta4 => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
        };
    }
}
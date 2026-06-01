namespace TelltaleToolKit.Serialization;

public static class MetaStreamMagicExtensions
{
    /// <summary>
    /// Converts a string to StreamVersion.
    /// </summary>
    public static MetaStreamMagic StreamVersionFromString(this string value)
    {
        return value switch
        {
            "MBIN" => MetaStreamMagic.Mbin,
            "MBES" => MetaStreamMagic.Mbes,
            "MTRE" => MetaStreamMagic.Mtre,
            "MCOM" => MetaStreamMagic.Mcom,
            "MSV4" => MetaStreamMagic.Msv4,
            "MSV5" => MetaStreamMagic.Msv5,
            "MSV6" => MetaStreamMagic.Msv6,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    /// <summary>
    /// Converts a StreamVersion to its string name.
    /// </summary>
    public static string ToJsonString(this MetaStreamMagic magic)
    {
        return magic switch
        {
            MetaStreamMagic.Mbin => "MBIN",
            MetaStreamMagic.Mbes => "MBES",
            MetaStreamMagic.Mcom => "MCOM",
            MetaStreamMagic.Mtre => "MTRE",
            MetaStreamMagic.Msv4 => "MSV4",
            MetaStreamMagic.Msv5 => "MSV5",
            MetaStreamMagic.Msv6 => "MSV6",
            _ => string.Empty
        };
    }
}

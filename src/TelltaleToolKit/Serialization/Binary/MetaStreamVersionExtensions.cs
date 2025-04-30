namespace TelltaleToolKit.Serialization.Binary;

public static class StreamVersionExtensions
{
    /// <summary>
    /// Converts a string to StreamVersion.
    /// </summary>
    public static MetaStreamVersion StreamVersionFromString(this string value)
    {
        return value switch
        {
            "MBIN" => MetaStreamVersion.Mbin,
            "MBES" => MetaStreamVersion.Mbes,
            "MTRE" => MetaStreamVersion.Mtre,
            "MCOM" => MetaStreamVersion.Mcom,
            "MSV4" => MetaStreamVersion.Msv4,
            "MSV5" => MetaStreamVersion.Msv5,
            "MSV6" => MetaStreamVersion.Msv6,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    /// <summary>
    /// Converts a StreamVersion to its string name.
    /// </summary>
    public static string ToJsonString(this MetaStreamVersion version)
    {
        return version switch
        {
            MetaStreamVersion.Mbin => "MBIN",
            MetaStreamVersion.Mbes => "MBES",
            MetaStreamVersion.Mcom => "MCOM",
            MetaStreamVersion.Mtre => "MTRE",
            MetaStreamVersion.Msv4 => "MSV4",
            MetaStreamVersion.Msv5 => "MSV5",
            MetaStreamVersion.Msv6 => "MSV6",
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
        };
    }
}
namespace TelltaleToolKit.GamesDatabase;

/// <summary>
/// Extension methods for <see cref="LuaVersion"/> to handle conversions between string representations and enum values.
/// </summary>
public static class LuaVersionExtensions
{
    /// <summary>
    /// Parses a string to its corresponding <see cref="LuaVersion"/> value.
    /// </summary>
    /// <param name="value">The string representation of the Lua version (e.g., "5.1.2", "5.2").</param>
    /// <returns>The matching <see cref="LuaVersion"/> value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the string does not match a supported version.</exception>
    public static LuaVersion ParseLuaVersion(this string value)
    {
        return value switch
        {
            "5.0.2" or "5.0" => LuaVersion.Lua502,
            "5.1.2" or "5.1" => LuaVersion.Lua512,
            "5.2.3" or "5.2" => LuaVersion.Lua523,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported Lua version string.")
        };
    }

    /// <summary>
    /// Gets the canonical string representation for a <see cref="LuaVersion"/> value.
    /// </summary>
    /// <param name="version">The <see cref="LuaVersion"/> value.</param>
    /// <returns>The string representation (e.g., "5.1.2").</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the enum value is not supported.</exception>
    public static string ToVersionString(this LuaVersion version)
    {
        return version switch
        {
            LuaVersion.Lua502 => "5.0.2",
            LuaVersion.Lua512 => "5.1.2",
            LuaVersion.Lua523 => "5.2.3",
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, "Unsupported LuaVersion enum value.")
        };
    }
}
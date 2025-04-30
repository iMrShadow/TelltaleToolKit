using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit.GamesDatabase;

/// <summary>
/// Represents a registry entry for a Telltale game configuration.
/// Each entry is identified by its file name following the convention:
/// <para>[slugified-title]-[year]-[month]-[platform]-[demo]</para>
/// This registry contains general information about the game, versioning, encryption, and a mapping of metaclasses.
/// </summary>
public sealed class GameDescriptor
{
    /// <summary>
    /// Gets or sets the unique identifier for the registry entry, derived from the file name.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets the display name of the game.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets a textual description of the game.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets the Blowfish encryption key used for securing game archives.
    /// In the config file, you can either use the name of <see cref="TelltaleToolKit.Utility.T3BlowfishKey"/> or a custom string.
    /// </summary>
    public string BlowfishKey { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the game uses the TTARCH2 archive format.
    /// </summary>
    public bool IsTtarch2 { get; init; }

    /// <summary>
    /// Gets the version of the TTARCH archive format used by the game.
    /// </summary>
    /// <seealso cref="ArchiveVersion"/>
    public ArchiveVersion TtarchVersion { get; init; }

    /// <summary>
    /// Gets the Lua scripting engine version used by the game.
    /// </summary>
    /// <seealso cref="LuaVersion"/>
    public LuaVersion LuaVersion { get; init; }

    /// <summary>
    /// Gets the version of the <see cref="MetaStream"/> stream format used in the game.
    /// </summary>
    /// <seealso cref="TelltaleToolKit.Serialization.Binary.MetaStreamVersion"/>
    public MetaStreamVersion MetaStreamVersion { get; init; }

    // TODO: Automatically assign false if MetaStreamVersion is MBIN.
    public bool AreSymbolsHashed { get; init; }

    /// <summary>
    /// Gets a value indicating whether Oodle compression is enabled for game archives.
    /// </summary>
    public bool EnableOodleCompression { get; init; }

    /// <summary>
    /// Gets a dictionary mapping <see cref="MetaClassType"/> to their corresponding IDs.
    /// This represents the metaclasses available in the game and their identifiers.
    /// </summary>
    public Dictionary<MetaClassType, uint> Classes { get; init; } = [];
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization;

/// <summary>
/// Holds configuration parameters and metadata for a MetaStream during serialization or deserialization.
/// </summary>
public class MetaStreamConfiguration
{
    /// <summary>
    /// Gets or sets the MetaStream version associated with this configuration.
    /// </summary>
    public uint StreamVersion { get; set; }

    /// <summary>
    /// True if all version CRCs match the compiled ones.
    /// </summary>
    public bool IsCompiledVersion { get; set; }

    /// <summary>
    /// Whether to apply legacy Blowfish block encryption (versions 2–3 only).
    /// For version 4+, encryption is handled per-section via ContainerStream.
    /// </summary>
    public bool Encrypt { get; set; }

    /// <summary>
    /// Whether to compress sections (versions 4+ only, via ContainerStream/Deflate).
    /// </summary>
    public bool Compress { get; set; }

    /// <summary>
    /// Gets the list of <see cref="MetaClass"/> objects for all classes that have been serialized in the current stream.
    /// </summary>
    public List<MetaVersionInfo> VersionInfo { get; set; } = [];

    /// <summary>
    /// Gets the list of serialized <see cref="Symbol"/> objects found in the stream.
    /// </summary>
    public List<Symbol> SerializedSymbols { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the list of serialized classes can be modified after initialization.
    /// </summary>
    public bool CanModifySerializedClassesList { get; set; }

    /// <summary>
    /// Gets the current workspace. Required for serializing if custom types are added.
    /// </summary>
    public Workspace? Workspace { get; set; }

    /// <summary>
    /// Gets the list of <see cref="MetaClass"/> objects for all classes that have been serialized in the current stream.
    /// </summary>
    public List<MetaClass> GetRegisteredClasses()
    {
        List<MetaClass> classes = [];

        foreach (MetaVersionInfo versionInfo in VersionInfo)
        {
            if (versionInfo.IsRegistered())
                classes.Add(versionInfo.GetMetaClass()!);
        }

        return classes;
    }

    /// <summary>
    /// Gets the list of classes found in the stream that are not registered in the current game profile.
    /// Each entry is a tuple containing the <see cref="MetaClassType"/> and its CRC32 checksum.
    /// </summary>
    public List<(MetaClassType Type, uint VersionCrc)> GetUnregisteredClasses()
    {
        List<(MetaClassType Type, uint VersionCrc)> unregistered = [];

        foreach (MetaVersionInfo versionInfo in VersionInfo)
        {
            if (!versionInfo.IsRegistered() && versionInfo.IsTypeRegistered())
                unregistered.Add((versionInfo.GetMetaClassType()!, versionInfo.VersionCrc));
        }

        return unregistered;
    }

    /// <summary>
    /// Gets the list of type hashes and checksums for types encountered in the stream that could not be resolved.
    /// Each entry is a tuple of the type hash (ulong) and CRC32 checksum.
    /// </summary>
    public List<(ulong TypeSymbolCrc, uint VersionCrc)> GetUnregisteredTypes()
    {
        List<(ulong TypeSymbolCrc, uint VersionCrc)> unregisteredTypes = [];

        foreach (var versionInfo in VersionInfo)
        {
            if (!versionInfo.IsTypeRegistered())
                unregisteredTypes.Add((versionInfo.TypeSymbolCrc, versionInfo.VersionCrc));
        }

        return unregisteredTypes;
    }

    /// <summary>
    /// Derives the correct on-disk magic FourCC from Version + Encrypt.
    /// Throws if the combination is invalid or not yet supported.
    /// </summary>
    internal MetaStreamMagic ResolveMagic()
    {
        // TODO: support MCOM and EncryptedMCOM, despite not being used in games
        return (StreamVersion, Encrypt) switch
        {
            (1, false) => MetaStreamMagic.Mbin,
            (1, true) => MetaStreamMagic.Mbes,
            (2, false) => MetaStreamMagic.Mbin, // Telltale used multiple;
            (2, true) => GetRandomEncryptedMbinMagic(), // Telltale used multiple;
            (3, false) => MetaStreamMagic.Mtre,
            (3, true) => MetaStreamMagic.EncryptedMtre,
            (4, (false)) => MetaStreamMagic.Msv4,
            (5, false) => MetaStreamMagic.Msv5,
            (6, false) => MetaStreamMagic.Msv6,
            _ => throw new NotSupportedException(
                $"No valid magic for Version={StreamVersion}, Encrypt={Encrypt}.")
        };
    }

    private static readonly Random s_random = new Random();
    private static readonly object s_lock = new object();

    private static MetaStreamMagic GetRandomEncryptedMbinMagic()
    {
        int choice;
        lock (s_lock)
        {
            choice = s_random.Next(0, 3); // 0, 1, or 2
        }
        return choice switch
        {
            0 => MetaStreamMagic.EncryptedMbin1,
            1 => MetaStreamMagic.EncryptedMbin2,
            2 => MetaStreamMagic.EncryptedMbin3,
            _ => MetaStreamMagic.EncryptedMbin1 // fallback
        };
    }
}

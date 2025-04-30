using TelltaleToolKit.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization.Binary;

/// <summary>
/// Holds configuration parameters and metadata for a MetaStream during serialization or deserialization.
/// </summary>
public class MetaStreamConfiguration
{
    /// <summary>
    /// Gets or sets the MetaStream version associated with this configuration.
    /// </summary>
    public MetaStreamVersion Version { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether symbols are stored as hashes instead of names.
    /// This exists only because MBIN is a hybrid version (Hashed in Sam and Max S2).
    /// </summary>
    public bool AreSymbolsHashed { get; set; }
    
    /// <summary>
    /// Gets the list of <see cref="MetaClass"/> objects for all classes that have been serialized in the current stream.
    /// </summary>
    public List<MetaClass> SerializedClasses { get; set; } = [];
    
    /// <summary>
    /// Gets the list of classes found in the stream that are not registered in the current context.
    /// Each entry is a tuple containing the <see cref="MetaClassType"/> and its CRC32 checksum.
    /// </summary>
    public List<(MetaClassType, uint crc32)> UnregisteredClasses { get; set; } = [];
    
    /// <summary>
    /// Gets the list of type hashes and checksums for types encountered in the stream that could not be resolved.
    /// Each entry is a tuple of the type hash (ulong) and CRC32 checksum.
    /// </summary>
    public List<(ulong, uint)> UnregisteredTypes { get; set; } = [];
    
    /// <summary>
    /// Gets the list of serialized <see cref="Symbol"/> objects found in the stream.
    /// </summary>
    public List<Symbol> SerializedSymbols { get; set; } = [];
    /// <summary>
    /// Gets or sets a value indicating whether the list of serialized classes can be modified after initialization.
    /// </summary>
    public bool CanModifySerializedClassesList { get; set; }
    
    // TODO: Don't access ActiveGameRegistry like that, improve the API.
    /// <summary>
    /// Provides a default static configuration based on the current active game registry.
    /// </summary>
    /// <remarks>
    /// <b>Warning:</b> This directly accesses <c>ActiveGameRegistry</c> on <see cref="TTKContext"/>.
    /// Consider improving the API to avoid tight coupling.
    /// </remarks>
    public static readonly MetaStreamConfiguration Default = new()
    {
        Version = TTKContext.Instance().ActiveGameRegistry.MetaStreamVersion,
        AreSymbolsHashed = TTKContext.Instance().ActiveGameRegistry.AreSymbolsHashed,
    };
}
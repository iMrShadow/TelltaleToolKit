using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

/// <summary>
/// Represents a single version info entry.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<MetaVersionInfo>))]
public class MetaVersionInfo
{
    [MetaMember("mTypeSymbolCrc")]
    public ulong TypeSymbolCrc { get; set; }

    [MetaMember("mVersionCrc")]
    public uint VersionCrc { get; set; }

    public bool IsRegistered()
        => Toolkit.Instance.ClassRegistry.GetClass(TypeSymbolCrc, VersionCrc) is not null;

    public bool IsTypeRegistered()
        => GetMetaClassType() is not null;

    public MetaClassType? GetMetaClassType()
        => MetaClassTypeRegistry.GetByHash(TypeSymbolCrc);

    public MetaClass? GetMetaClass()
        => Toolkit.Instance.ClassRegistry.GetClass(TypeSymbolCrc, VersionCrc);
}

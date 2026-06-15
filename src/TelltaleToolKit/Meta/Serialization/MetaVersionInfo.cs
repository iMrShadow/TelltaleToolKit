using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.Meta.Serialization;

/// <summary>
/// Represents a single version info entry.
/// </summary>
[MetaSerializer(typeof(MetaClassSerializer<MetaVersionInfo>))]
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

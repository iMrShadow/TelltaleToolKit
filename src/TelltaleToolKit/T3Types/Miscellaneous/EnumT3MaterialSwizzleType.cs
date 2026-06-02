using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3MaterialSwizzleType>))]
public struct EnumT3MaterialSwizzleType
{
    [MetaMember("mVal")]
    public T3MaterialSwizzleType Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<T3MaterialSwizzleType>))]
public enum T3MaterialSwizzleType
{
    // TODO:
}

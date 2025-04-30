using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3MaterialSwizzleType>))]
public struct EnumT3MaterialSwizzleType
{
    [MetaMember("mVal")]
    public T3MaterialSwizzleType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialSwizzleType>))]
public enum T3MaterialSwizzleType
{
    // TODO:
}
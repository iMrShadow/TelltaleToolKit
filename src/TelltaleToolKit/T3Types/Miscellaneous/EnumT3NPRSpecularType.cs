using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3NPRSpecularType>))]
public struct EnumT3NPRSpecularType
{
    [MetaMember("mVal")]
    public T3NPRSpecularType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3NPRSpecularType>))]
public enum T3NPRSpecularType
{
    // TODO:
}

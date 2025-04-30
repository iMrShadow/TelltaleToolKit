using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3DetailShadingType>))]
public struct EnumT3DetailShadingType
{
    [MetaMember("mVal")]
    public T3DetailShadingType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3DetailShadingType>))]
public enum T3DetailShadingType
{
    // TODO:
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvShadowType>))]
public struct EnumT3LightEnvShadowType
{
    [MetaMember("mVal")]
    public T3LightEnvShadowType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvShadowType>))]
public enum T3LightEnvShadowType
{
    //    eLightEnvShadowType_
    None = 0,
    PerLight = 2,
    Modulated = 3,
}
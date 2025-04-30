using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvShadowQuality>))]
public struct EnumT3LightEnvShadowQuality
{
    [MetaMember("mVal")]
    public T3LightEnvShadowQuality Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvShadowQuality>))]
public enum T3LightEnvShadowQuality
{
    // eLightEnvShadowQuality_
    Low = 0,
    Medium = 1,
    High = 2,
}
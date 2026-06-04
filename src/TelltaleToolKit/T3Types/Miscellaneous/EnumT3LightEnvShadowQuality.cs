using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvShadowQuality>))]
public struct EnumT3LightEnvShadowQuality
{
    [MetaMember("mVal")]
    public T3LightEnvShadowQuality Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvShadowQuality>))]
    public enum T3LightEnvShadowQuality
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}

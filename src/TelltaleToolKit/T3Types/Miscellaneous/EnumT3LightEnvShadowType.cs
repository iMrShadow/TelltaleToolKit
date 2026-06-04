using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvShadowType>))]
public struct EnumT3LightEnvShadowType
{
    [MetaMember("mVal")]
    public T3LightEnvShadowType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvShadowType>))]
    public enum T3LightEnvShadowType
    {
        None = 0,
        PerLight = 2,
        Modulated = 3
    }
}

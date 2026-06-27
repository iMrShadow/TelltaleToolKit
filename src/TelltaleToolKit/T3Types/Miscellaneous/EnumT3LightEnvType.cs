using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvType>))]
public struct EnumT3LightEnvType
{
    [MetaMember("mVal")]
    public T3LightEnvType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvType>))]
    public enum T3LightEnvType
    {
        Point = 0x0,
        Spot = 0x1,
        DirectionalKey = 0x2,
        Ambient = 0x3,
        DirectionalAmbient = 0x4
    }
}

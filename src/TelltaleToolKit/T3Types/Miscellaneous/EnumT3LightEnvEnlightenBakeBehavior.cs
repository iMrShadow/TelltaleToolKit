using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvEnlightenBakeBehavior>))]
public struct EnumT3LightEnvEnlightenBakeBehavior
{
    [MetaMember("mVal")]
    public T3LightEnvEnlightenBakeBehavior Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvEnlightenBakeBehavior>))]
    public enum T3LightEnvEnlightenBakeBehavior
    {
        Auto = 0x0,
        Enable = 0x1,
        Disable = 0x2
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvLodBehavior>))]
public struct EnumT3LightEnvLodBehavior
{
    [MetaMember("mVal")]
    public T3LightEnvLODBehavior Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvLODBehavior>))]
    public enum T3LightEnvLODBehavior
    {
        Disable = 0x0,
        BakeOnly = 0x1
    }
}

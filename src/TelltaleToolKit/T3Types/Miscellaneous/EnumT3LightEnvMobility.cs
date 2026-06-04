using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvMobility>))]
public struct EnumT3LightEnvMobility
{
    [MetaMember("mVal")]
    public T3LightEnvMobility Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvMobility>))]
    public enum T3LightEnvMobility
    {
        Static = 0,
        Stationary = 1,
        Moveable = 2
    }
}

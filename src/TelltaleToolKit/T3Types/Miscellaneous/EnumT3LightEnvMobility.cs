using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvMobility>))]
public struct EnumT3LightEnvMobility
{
    [MetaMember("mVal")]
    public T3LightEnvMobility Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvMobility>))]
public enum T3LightEnvMobility
{
    //     eLightEnvMobility_
    Static = 0,
    Stationary = 1,
    Moveable = 2,
}
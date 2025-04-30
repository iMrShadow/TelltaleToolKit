using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvLodBehavior>))]
public struct EnumT3LightEnvLodBehavior
{
    [MetaMember("mVal")]
    public T3LightEnvLODBehavior Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvLODBehavior>))]
public enum T3LightEnvLODBehavior
{
    //    eLightEnvLOD_
    Disable = 0x0,
    BakeOnly = 0x1,
}
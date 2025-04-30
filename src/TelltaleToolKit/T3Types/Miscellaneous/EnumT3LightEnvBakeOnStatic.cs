using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvBakeOnStatic>))]
public struct EnumT3LightEnvBakeOnStatic
{
    [MetaMember("mVal")]
    public T3LightEnvBakeOnStatic Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvBakeOnStatic>))]
public enum T3LightEnvBakeOnStatic
{
    // TODO:
}

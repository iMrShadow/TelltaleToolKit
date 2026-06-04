using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3LightEnvBakeOnStatic>))]
public struct EnumT3LightEnvBakeOnStatic
{
    [MetaMember("mVal")]
    public T3LightEnvBakeOnStatic Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3LightEnvBakeOnStatic>))]
    public enum T3LightEnvBakeOnStatic
    {
        Default = 0x0,
        AlwaysAllow = 0x1,
        NeverAllow = 0x2
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<T3LightEnvLOD>))]
public struct T3LightEnvLOD
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    // TODO:
    [Flags]
    public enum T3LightEnvLODFlags
    {
        // eLOD
        High = 0x10000,
        Medium = 0x20000,
        Low = 0x40000
    }
}

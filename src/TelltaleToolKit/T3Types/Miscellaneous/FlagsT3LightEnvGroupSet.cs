using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<FlagsT3LightEnvGroupSet>))]
public struct FlagsT3LightEnvGroupSet
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}

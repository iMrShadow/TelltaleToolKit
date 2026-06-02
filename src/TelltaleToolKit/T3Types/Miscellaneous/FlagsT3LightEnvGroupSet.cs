using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<FlagsT3LightEnvGroupSet>))]
public struct FlagsT3LightEnvGroupSet
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}

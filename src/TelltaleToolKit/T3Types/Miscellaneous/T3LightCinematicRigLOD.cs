using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3LightCinematicRigLOD>))]
public struct T3LightCinematicRigLOD
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}

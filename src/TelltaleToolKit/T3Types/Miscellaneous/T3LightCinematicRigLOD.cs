using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3LightCinematicRigLOD>))]
public struct T3LightCinematicRigLOD
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}
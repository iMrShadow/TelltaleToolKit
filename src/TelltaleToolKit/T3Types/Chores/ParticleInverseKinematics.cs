using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<ParticleInverseKinematics>))]
public class ParticleInverseKinematics
{
    [MetaMember("Baseclass_InverseKinematicsBase")]
    public InverseKinematicsBase Baseclass_InverseKinematicsBase { get; set; } = new();
}

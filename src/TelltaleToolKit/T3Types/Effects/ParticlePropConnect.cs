using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Effects;

[MetaSerializer(typeof(MetaClassSerializer<ParticlePropConnect>))]
public class ParticlePropConnect
{
    [MetaMember("mModType")]
    public EnumParticlePropModifier.ParticlePropModifier ModType { get; set; }

    [MetaMember("mDriveType")]
    public EnumParticlePropDriver.ParticlePropDriver DriveType { get; set; }

    [MetaMember("mDriveMin")]
    public float DriveMin { get; set; }

    [MetaMember("mDriveMax")]
    public float DriveMax { get; set; }

    [MetaMember("mModMin")]
    public float ModMin { get; set; }

    [MetaMember("mModMax")]
    public float ModMax { get; set; }

    [MetaMember("mInvert")]
    public bool Invert { get; set; }
}

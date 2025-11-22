using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Effects;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ParticlePropConnect>))]
public class ParticlePropConnect
{
    [MetaMember("mModType")]
    public ParticlePropModifier mModType { get; set; }

    [MetaMember("mDriveType")]
    public ParticlePropDriver mDriveType { get; set; }

    [MetaMember("mDriveMin")]
    public float mDriveMin { get; set; }

    [MetaMember("mDriveMax")]
    public float mDriveMax { get; set; }

    [MetaMember("mModMin")]
    public float mModMin { get; set; }

    [MetaMember("mModMax")]
    public float mModMax { get; set; }

    [MetaMember("mInvert")]
    public bool mInvert { get; set; }
}
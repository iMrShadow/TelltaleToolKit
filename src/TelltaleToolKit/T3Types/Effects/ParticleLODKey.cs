using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Effects;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ParticleLODKey>))]
public class ParticleLODKey
{
    [MetaMember("mCountScale")]
    public float mCountScale { get; set; }

    [MetaMember("mStrideScale")]
    public float mStrideScale { get; set; }

    [MetaMember("mDivisionScale")]
    public float mDivisionScale { get; set; }

    [MetaMember("mLifeScale")]
    public float mLifeScale { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Effects;

[MetaSerializer(typeof(MetaClassSerializer<ParticleLODKey>))]
public class ParticleLODKey
{
    [MetaMember("mCountScale")]
    public float CountScale { get; set; }

    [MetaMember("mStrideScale")]
    public float StrideScale { get; set; }

    [MetaMember("mDivisionScale")]
    public float DivisionScale { get; set; }

    [MetaMember("mLifeScale")]
    public float LifeScale { get; set; }
}

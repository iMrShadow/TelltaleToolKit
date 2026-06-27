using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<BlendEntry>))]
public class BlendEntry
{
    [MetaMember("mParameterValues")]
    public Vector3 ParameterValues { get; set; }

    [MetaMember("mAnimOrChore")]
    public AnimOrChore AnimOrChore { get; set; }

    [MetaMember("mCorrespondencePoints")]
    public List<CorrespondencePoint> CorrespondencePoints { get; set; } = [];

    [MetaMember("mfAnimOrChoreLength")]
    public float AnimOrChoreLength { get; set; }

    [MetaMember("mComment")]
    public string Comment { get; set; } = string.Empty;
}

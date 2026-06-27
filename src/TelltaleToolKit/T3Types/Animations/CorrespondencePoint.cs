using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<CorrespondencePoint>))]
public class CorrespondencePoint
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mEaseOutStartFlags")]
    public Flags EaseOutStartFlags { get; set; }

    [MetaMember("mEaseOutEndFlags")]
    public Flags EaseOutEndFlags { get; set; }

    [MetaMember("mEaseInStartFlags")]
    public Flags EaseInStartFlags { get; set; }

    [MetaMember("mEaseInEndFlags")]
    public Flags EaseInEndFlags { get; set; }

    [MetaMember("mSteeringFlags")]
    public Flags SteeringFlags { get; set; }

    [MetaMember("mTransitionFlags")]
    public Flags TransitionFlags { get; set; }

    [MetaMember("mfTime")]
    public float Time { get; set; }

    [MetaMember("mComment")]
    public string Comment { get; set; }
}

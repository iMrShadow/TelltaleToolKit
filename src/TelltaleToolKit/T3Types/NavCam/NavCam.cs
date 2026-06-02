using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.NavCam;

[MetaSerializer(typeof(EnumSerializer<Mode>))]
public enum Mode
{
    None = 1,
    LookAt = 2,
    Orbit = 3,
    AnimationTrack = 4,
    AnimationTime = 5,
    AnimationPosProcedualLookAt = 6,
    ScenePosition = 7,
    DynamicConversationCamera = 8
}

[MetaSerializer(typeof(MetaClassSerializer<EnumMode>))]
public struct EnumMode
{
    [MetaMember("mVal")]
    public Mode Val { get; set; }
}

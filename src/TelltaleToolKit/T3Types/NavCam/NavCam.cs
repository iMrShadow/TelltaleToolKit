using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.NavCam;

[MetaClassSerializerGlobal(typeof(EnumSerializer<Mode>))]
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

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumMode>))]
public struct EnumMode
{
    [MetaMember("mVal")]
    public Mode Val { get; set; }
}
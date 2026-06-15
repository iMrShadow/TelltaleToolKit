using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<AnimationValueInterfaceBase>))]
public class AnimationValueInterfaceBase
{
    // Both Symbol and a String
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mFlags")]
    public int Flags { get; set; }

    public short Version { get; set; } = 0;
}

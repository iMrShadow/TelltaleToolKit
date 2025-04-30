using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AnimationValueInterfaceBase>))]
public class AnimationValueInterfaceBase
{
    // Both Symbol and a String
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mFlags")]
    public int Flags { get; set; }

    public short Version { get; set; } = 0;
}
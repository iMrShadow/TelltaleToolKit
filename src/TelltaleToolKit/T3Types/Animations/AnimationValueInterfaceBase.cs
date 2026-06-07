using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<AnimationValueInterfaceBase>))]
public class AnimationValueInterfaceBase
{
    [MetaMember("mName")]
    public Symbol NameS { get; set; } = Symbol.Empty;

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mFlags")]
    public int Flags { get; set; }

    [MetaMember("mExtrapolationModeBefore")]
    public int ExtrapolationModeBefore { get; set; }

    [MetaMember("mExtrapolationModeAfter")]
    public int ExtrapolationModeAfter { get; set; }

    public short Version { get; set; } = 0;
}

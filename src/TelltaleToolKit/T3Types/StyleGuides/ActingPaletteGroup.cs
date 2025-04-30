using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ActingPaletteGroup>))]
public class ActingPaletteGroup
{
    [MetaMember("Baseclass_UID::Owner")]
    public Owner BaseClassOwner { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; }

    [MetaMember("mIdle")]
    public AnimOrChore Idle { get; set; }

    [MetaMember("mWeight")]
    public float Weight { get; set; }

    [MetaMember("mTransitionIn")]
    public AnimOrChore TransitionIn { get; set; } = new();

    [MetaMember("mTransitionOut")]
    public AnimOrChore TransitionOut { get; set; }

    public class ActingPaletteTransition
    {
    }

    public class EnumIdleTransition
    {
    }
}
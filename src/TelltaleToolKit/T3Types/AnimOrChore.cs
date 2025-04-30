using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AnimOrChore>))]
public class AnimOrChore
{
    [MetaMember("mhAnim")]
    public Handle<Animation> Animation { get; set; } = new();

    [MetaMember("mhChore")]
    public Handle<Chore> Chore { get; set; } = new();
}
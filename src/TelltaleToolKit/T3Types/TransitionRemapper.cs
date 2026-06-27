using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<TransitionRemapper>))]
public class TransitionRemapper
{
    [MetaMember("mRemapKeys")]
    public KeyframedValue<float> RemapKeys { get; set; } = new();
}

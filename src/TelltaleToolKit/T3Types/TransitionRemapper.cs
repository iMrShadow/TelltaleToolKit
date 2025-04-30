using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TransitionRemapper>))]
public class TransitionRemapper
{
    [MetaMember("mRemapKeys")]
    public KeyframedValue<float> RemapKeys = new();
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<KeyframedValueSteppedString>))]
public class KeyframedValueSteppedString
{
    [MetaMember("Baseclass_KeyframedValue<String>")]
    public KeyframedValue<string> BaseClassKeyframedValue { get; set; } = new();
}

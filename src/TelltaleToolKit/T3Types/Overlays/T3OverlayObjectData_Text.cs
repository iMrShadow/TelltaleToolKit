using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaSerializer(typeof(MetaClassSerializer<T3OverlayObjectDataText>))]
public class T3OverlayObjectDataText
{
    [MetaMember("mName")]
    public Symbol Name { get; set; } = Symbol.Empty;

    [MetaMember("mParams")]
    public T3OverlayTextParams Parameters { get; set; } = new();
}

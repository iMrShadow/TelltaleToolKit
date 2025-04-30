using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OverlayObjectDataText>))]
public class T3OverlayObjectDataText
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mParams")]
    public T3OverlayTextParams Parameters { get; set; } = new();
}
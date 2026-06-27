using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaSerializer(typeof(MetaClassSerializer<T3OverlayObjectDataSprite>))]
public class T3OverlayObjectDataSprite
{
    [MetaMember("mName")]
    public Symbol Name { get; set; } = Symbol.Empty;

    [MetaMember("mParams")]
    public T3OverlaySpriteParams Parameters { get; set; } = new();
}

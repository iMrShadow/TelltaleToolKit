using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OverlayObjectDataSprite>))]
public class T3OverlayObjectDataSprite
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mParams")]
    public T3OverlaySpriteParams Parameters { get; set; }
}
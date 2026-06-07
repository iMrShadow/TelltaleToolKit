using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

/// <summary>
///     Main class for .overlay files.
/// </summary>
[MetaSerializer(typeof(MetaClassSerializer<T3OverlayData>))]
public class T3OverlayData
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mSpriteObjects")]
    public List<T3OverlayObjectDataSprite> SpriteObjects { get; set; } = [];

    [MetaMember("mTextObjects")]
    public List<T3OverlayObjectDataText> TextObjects { get; set; } = [];

    [MetaMember("mParams")]
    public T3OverlayParams Parameters { get; set; } = new();
}

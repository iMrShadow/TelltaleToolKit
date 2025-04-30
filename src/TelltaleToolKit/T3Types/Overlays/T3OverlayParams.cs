using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OverlayParams>))]
public class T3OverlayParams
{
    [MetaMember("mhBackgroundTexture")]
    public Handle<T3Texture> BackgroundTexture { get; set; } = new();

    [MetaMember("mhChore")]
    public Handle<Chore> Chore { get; set; } = new();

    [MetaMember("mMinDisplayTime")]
    public float MinDisplayTime { get; set; }

    [MetaMember("mFadeTime")]
    public float FadeTime { get; set; }
}
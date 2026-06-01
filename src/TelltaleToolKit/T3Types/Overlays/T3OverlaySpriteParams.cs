using System.Numerics;
using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OverlaySpriteParams>))]
public class T3OverlaySpriteParams
{
    [MetaMember("mhSprite")]
    public Handle<ParticleSprite> Sprite { get; set; }

    [MetaMember("mInitialPosition")]
    public Vector2 InitialPosition { get; set; }

    [MetaMember("mSize")]
    public Vector2 Size { get; set; }

    [MetaMember("mAnimation")]
    public Symbol Animation { get; set; }

    [MetaMember("mAnimationSpeed")]
    public float AnimationSpeed { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}
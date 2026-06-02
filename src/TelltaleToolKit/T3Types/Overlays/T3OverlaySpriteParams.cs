using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaSerializer(typeof(MetaClassSerializer<T3OverlaySpriteParams>))]
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

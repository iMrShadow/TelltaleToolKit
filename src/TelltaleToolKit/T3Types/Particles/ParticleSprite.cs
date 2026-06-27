using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Meshes.T3Types;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Particles;

[MetaSerializer(typeof(MetaClassSerializer<ParticleSprite>))]
public class ParticleSprite
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mhTexture")]
    public Handle<T3Texture>[] Texture { get; set; } = [];

    [MetaMember("mTextureX")]
    public int TextureX { get; set; }

    [MetaMember("mTextureY")]
    public int TextureY { get; set; }

    [MetaMember("mSpriteSize")]
    public Vector2 SpriteSize { get; set; }

    [MetaMember("mBlendMode")]
    public BlendMode BlendMode { get; set; }

    [MetaMember("mAnimations")]
    public List<Animation> Animations { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<Animation>))]
    public class Animation
    {
        [MetaMember("mName")]
        public Symbol Name { get; set; } = Symbol.Empty;

        [MetaMember("mStartFrame")]
        public int StartFrame { get; set; }

        [MetaMember("mFrameCount")]
        public int FrameCount { get; set; }
    }
}

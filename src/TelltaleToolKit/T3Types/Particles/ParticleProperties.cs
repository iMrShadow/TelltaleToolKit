using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Particles;

[MetaSerializer(typeof(Serializer))]
public class ParticleProperties
{
    [Flags]
    public enum AnimationDataFlags
    {
        Chore = 1, //Animation::mpChore must not be null!
        Samples = 2 //Animation::mpSamples must not be null!
    }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mTextureFlags")]
    public Flags TextureFlags { get; set; }

    [MetaMember("mTextureCount")]
    public int TextureCount { get; set; }

    [MetaMember("mAnimations")]
    public List<Animation> Animations { get; set; } = [];

    public T3Texture[] Texture { get; set; } = new T3Texture[4];

    [MetaSerializer(typeof(MetaClassSerializer<Animation>))]
    public class Animation
    {
        [MetaMember("mName")]
        public Symbol Name { get; set; } = Symbol.Empty;

        [MetaMember("mParams")]
        public AnimationParams Params { get; set; } = new();

        [MetaMember("mDataFlags")]
        public Flags DataFlags { get; set; }

        public Chore? Chore { get; set; }
        public ParticlePropertySamples Samples { get; set; } = new();
    }

    [MetaSerializer(typeof(MetaClassSerializer<AnimationParams>))]
    public class AnimationParams
    {
        [MetaMember("mPositionMin")]
        public Vector3 PositionMin { get; set; }

        [MetaMember("mPositionMax")]
        public Vector3 PositionMax { get; set; }
    }

    public class Serializer : MetaSerializer<ParticleProperties>
    {
        private static readonly MetaClassSerializer<ParticleProperties> s_metaClassSerializer = new();

        public override void PreSerialize(ref ParticleProperties? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new ParticleProperties();

        public override void Serialize(ref ParticleProperties obj, MetaStream stream, MetaClassType? type = null)
        {
            int enumCount = Enum.GetValues(typeof(ParticlePropertiesTexture)).Length;

            if (stream.Mode is MetaStreamMode.Write)
            {
                obj.TextureCount = 0;
                obj.TextureFlags &= 0xF;

                for (int i = 0; i < enumCount; i++)
                {
                    if (obj.Texture[i] is not null)
                    {
                        obj.TextureFlags |= 1 << i;
                    }
                    else
                    {
                        obj.TextureFlags &= ~(1 << i);
                    }

                    if ((obj.TextureFlags.Data & (1 << i)) != 0)
                    {
                        obj.TextureCount++;
                    }
                }
            }

            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                foreach (Animation animation in obj.Animations)
                {
                    if (animation.DataFlags.Has((int)AnimationDataFlags.Chore))
                    {
                        // Chore must exist - serialize it
                        Chore? animationChore = animation.Chore;
                        if (animationChore == null)
                        {
                            throw new InvalidOperationException(
                                $"Animation '{animation.Name}' has Chore flag but Chore is null");
                        }

                        stream.Serialize(ref animationChore);
                    }
                    else if (animation.DataFlags.Has((int)AnimationDataFlags.Samples))
                    {
                        // Samples must exist - serialize it
                        ParticlePropertySamples particlePropertySamples = animation.Samples;
                        stream.Serialize(ref particlePropertySamples);
                    }
                }

                // WRITE: Textures based on flags
                if (obj.TextureCount <= 0)
                {
                    return;
                }

                for (int i = 0; i < enumCount; i++)
                {
                    if ((obj.TextureFlags & (1 << i)) != 0)
                    {
                        T3Texture? texture = obj.Texture[i];
                        if (texture == null)
                        {
                            throw new InvalidOperationException(
                                $"Texture at index {i} is marked as present but is null");
                        }

                        stream.Serialize(ref texture);
                    }
                }
            }
            else
            {
                foreach (Animation animation in obj.Animations)
                {
                    if (animation.DataFlags.Has((int)AnimationDataFlags.Chore))
                    {
                        Chore chore = new();
                        stream.Serialize(ref chore);
                        animation.Chore = chore;
                    }
                    else if (animation.DataFlags.Has((int)AnimationDataFlags.Samples))
                    {
                        ParticlePropertySamples samples = new();
                        stream.Serialize(ref samples);
                        animation.Samples = samples;
                    }
                }

                for (int i = 0; i < obj.TextureCount; i++)
                {
                    if ((obj.TextureFlags & (1 << i)) != 0)
                    {
                        T3Texture texture = new();
                        stream.Serialize(ref texture);
                        obj.Texture[i] = texture;
                    }
                }
            }
        }
    }
}

public enum ParticlePropertiesTexture
{
    Position = 0,
    Orientation = 1,
    Color = 2,
    Rotation3D = 3
}

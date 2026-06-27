using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class BlendGraph
{
    public object Geometry;

    [MetaMember("mParameters")]
    public List<Symbol> Parameters { get; set; } = [];

    [MetaMember("mFrozenParameterNames")]
    public List<Symbol> FrozenParameterNames { get; set; } = [];

    [MetaMember("mDampeningConstants")]
    public List<float> DampeningConstants { get; set; } = [];

    [MetaMember("mEntries")]
    public List<BlendEntry> Entries { get; set; } = [];

    [MetaMember("mBlendGraphType")]
    public EnumBlendGraphType BlendGraphTypeT { get; set; }

    [MetaMember("mDampen")]
    public bool Dampen { get; set; }

    [MetaMember("mbInvertParameters")]
    public int InvertParameters { get; set; }

    [MetaMember("mfTimeScale")]
    public float TimeScale { get; set; }

    [MetaMember("mComment")]
    public string Comment { get; set; } = string.Empty;

    [MetaMember("mhBlendGraphAuxiliaryChore")]
    public Handle<Chore> BlendGraphAuxiliaryChore { get; set; }

    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mNumGeometryDimensions")]
    public int NumGeometryDimensions { get; set; }

    [MetaMember("mNumDimensions")]
    public int NumDimensions { get; set; }

    [MetaMember("mParameterOrder")]
    public List<int> ParameterOrder { get; set; } = [];


    [MetaSerializer(typeof(EnumSerializer<BlendGraphType>))]
    public enum BlendGraphType
    {
        Looping = 0,
        NonLooping = 1
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumBlendGraphType>))]
    public struct EnumBlendGraphType
    {
        [MetaMember("mVal")]
        public BlendGraphType Val { get; set; }
    }

    public class Serializer : MetaSerializer<BlendGraph>
    {
        private static readonly MetaClassSerializer<BlendGraph> s_metaClassSerializer = new();

        public override void Serialize(ref BlendGraph obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                bool hasGeometry = stream.ReadBoolean();

                if (!hasGeometry)
                {
                    return;
                }

                // What the hell

                Type type1 = obj.NumDimensions switch
                {
                    1 => typeof(KeyframedValue<int>),
                    2 => typeof(Dictionary<float, KeyframedValue<int>>),
                    3 => typeof(Dictionary<float, Dictionary<float, KeyframedValue<int>>>),
                    _ => throw new ArgumentOutOfRangeException()
                };

                obj.Geometry = obj.NumDimensions switch
                {
                    1 => new KeyframedValue<int>(),
                    2 => new Dictionary<float, KeyframedValue<int>>(),
                    3 => new Dictionary<float, Dictionary<float, KeyframedValue<int>>>(),
                    _ => obj.Geometry
                };

                MetaSerializer typeSerializer = Toolkit.Instance.GetSerializer(type1);
                typeSerializer.PreSerialize(ref obj.Geometry, stream);
                typeSerializer.Serialize(ref obj.Geometry, stream);
            }
        }
    }
}

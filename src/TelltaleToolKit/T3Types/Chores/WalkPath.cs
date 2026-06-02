using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(Serializer))]
public class WalkPath
{
    [MetaMember("mName")]
    public string Name { get; set; }

    public List<PathBase> Paths { get; set; }
    public class Serializer : MetaSerializer<WalkPath>
    {
        private static readonly MetaClassSerializer<WalkPath> s_metaClassSerializer = new();

        public override void Serialize(ref WalkPath obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException($"There is no serializer for {SerializationType}");
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int count = stream.ReadInt32();
                MetaClassType? type = stream.ReadMetaClassType();
                if (type is null)
                    throw new InvalidOperationException("[WalkPath] Type is not registered.");

                MetaSerializer typeSerializer =
                    Toolkit.Instance.GetSerializer(type.LinkingType);

                for (var i = 0; i < count; i++)
                {
                    object? value = null;

                    typeSerializer.PreSerialize(ref value, stream, type);
                    typeSerializer.Serialize(ref value, stream);

                    if (value is PathBase pathBase)
                    {
                        obj.Paths.Add(pathBase);
                    }
                }
            }
        }
    }
}

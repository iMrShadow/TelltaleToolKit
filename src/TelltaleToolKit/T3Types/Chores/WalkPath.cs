using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class WalkPath
{
    [MetaMember("mName")]
    public string Name { get; set; }
    
    public List<PathBase> Paths { get; set; }
    public class Serializer : MetaClassSerializer<WalkPath>
    {
        private static readonly DefaultClassSerializer<WalkPath> DefaultSerializer = new();

        public override void Serialize(ref WalkPath obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter)
            {
                throw new NotImplementedException($"There is no serializer for {SerializationType}");
            }

            if (stream is MetaStreamReader streamReader)
            {
                int count = streamReader.ReadInt32();
                MetaClassType type = streamReader.ReadMetaClassType();
                MetaClassSerializer classTypeSerializer =
                    T3Kit.Instance.GetSerializer(type.LinkingType);
                
                for (var i = 0; i < count; i++)
                {
                    object? value = null;

                    classTypeSerializer.PreSerialize(ref value, stream, type);
                    classTypeSerializer.Serialize(ref value, stream);

                    if (value is PathBase pathBase)
                    {
                        obj.Paths.Add(pathBase);
                    }
                }
            }
        }
    }
}
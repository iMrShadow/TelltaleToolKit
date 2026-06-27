using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(Serializer))]
public class WalkPath
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    public List<PathBase> Paths { get; set; } = [];

    public class Serializer : MetaSerializer<WalkPath>
    {
        private static readonly MetaClassSerializer<WalkPath> s_metaClassSerializer = new();

        public override void PreSerialize(ref WalkPath? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new WalkPath();
        }

        public override void Serialize(ref WalkPath obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Paths.Count);
                foreach (var path in obj.Paths)
                {
                    PathBase pathBase = path;

                    var mcd = stream.GetMetaClass(pathBase.GetType());
                    if (mcd is null)
                        throw new InvalidOperationException("[WalkPath] MetaClass for PathBase is not supported.");

                    stream.Write(mcd.ClassType.Symbol.Crc64);
                    stream.Serialize(ref pathBase);
                }
            }
            else
            {
                int count = stream.ReadInt32();
                MetaClassType? typeS = stream.ReadMetaClassType();
                if (typeS is null)
                    throw new InvalidOperationException("[WalkPath] Type is not registered.");

                MetaSerializer typeSerializer =
                    Toolkit.Instance.GetSerializer(typeS.LinkingType);

                for (int i = 0; i < count; i++)
                {
                    object? value = null;

                    typeSerializer.PreSerialize(ref value!, stream, typeS);
                    typeSerializer.Serialize(ref value, stream, typeS);

                    if (value is PathBase pathBase)
                    {
                        obj.Paths.Add(pathBase);
                    }
                }
            }
        }
    }
}

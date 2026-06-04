using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<FileNameBase>))]
public class FileNameBase
{
    [MetaMember("mFileName")]
    public Symbol FileName { get; set; } = Symbol.Empty;
}

[MetaSerializer(typeof(FileName<>.Serializer), typeof(FileName<>))]
public class FileName<T>
{
    [MetaMember("Baseclass_FileNameBase")]
    public FileNameBase FileNameBase { get; set; } = new();

    public class Serializer : MetaSerializer<FileName<T>>
    {
        private static readonly MetaClassSerializer<FileName<T>> s_metaClassSerializer = new();

        public override void PreSerialize(ref FileName<T>? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new FileName<T>();

        public override void Serialize(ref FileName<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);
        }
    }
}

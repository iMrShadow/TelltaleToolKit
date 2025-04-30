using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<FileNameBase>))]
public class FileNameBase
{
    [MetaMember("mFileName")]
    public Symbol FileName { get; set; }
}

[MetaClassSerializerGlobal(typeof(FileName<>.Serializer), typeof(FileName<>))]
public class FileName<T>
{
    [MetaMember("Baseclass_FileNameBase")]
    public FileNameBase FileNameBase { get; set; } = new();

    public class Serializer : MetaClassSerializer<FileName<T>>
    {
        private static readonly DefaultClassSerializer<FileName<T>> DefaultSerializer = new();

        public override void PreSerialize(ref FileName<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new FileName<T>();
            }
        }

        public override void Serialize(ref FileName<T> obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);
        }
    }
}
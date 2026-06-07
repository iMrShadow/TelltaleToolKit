using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.ActorMaps;

[MetaSerializer(typeof(T3VertexBufferSample<>.Serializer), typeof(T3VertexBufferSample<>))]
public class T3VertexBufferSample<T> where T : T3VertexSampleDatabase
{
    public T3VertexSampleDatabase Data { get; set; } = new();

    public class Serializer : MetaSerializer<T3VertexBufferSample<T>>
    {
        private static readonly MetaClassSerializer<T3VertexSampleDatabase> s_metaClassSerializer = new();

        public override void PreSerialize(ref T3VertexBufferSample<T>? obj, MetaStream stream,
            MetaClassType? type = null)
        {
            obj ??= new T3VertexBufferSample<T>();
        }

        public override void Serialize(ref T3VertexBufferSample<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            T3VertexSampleDatabase t3VertexSampleDatabase = obj.Data;
            s_metaClassSerializer.PreSerialize(ref t3VertexSampleDatabase!, stream);
            s_metaClassSerializer.Serialize(ref t3VertexSampleDatabase, stream);
        }
    }
}

[MetaSerializer(typeof(Serializer))]
public class T3VertexSampleDatabase
{
    [MetaMember("mNumVerts")]
    public int NumVertices { get; set; }

    [MetaMember("mVertSize")]
    public int VertSize { get; set; }

    public class Serializer : MetaSerializer<T3VertexSampleDatabase>
    {
        private static readonly MetaClassSerializer<T3VertexSampleDatabase> s_metaClassSerializer = new();

        public override void PreSerialize(ref T3VertexSampleDatabase? obj, MetaStream stream,
            MetaClassType? type = null)
        {
            obj ??= new T3VertexSampleDatabase();
        }

        public override void Serialize(ref T3VertexSampleDatabase obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else
            {
                 byte[] data = new byte[obj.NumVertices * obj.VertSize];
                 stream.ReadBytes(data);
                 // TODO:
            }
        }
    }
}

[MetaSerializer(typeof(Serializer))]
public class T3NormalSampleData : T3VertexSampleDatabase
{
    public new class Serializer : MetaSerializer<T3NormalSampleData>
    {
        public override void Serialize(ref T3NormalSampleData obj, MetaStream stream, MetaClassType? type = null)
        {
            T3VertexSampleDatabase objData = obj;
            stream.Serialize(ref objData);
            obj = (T3NormalSampleData)objData;
        }
    }
}

[MetaSerializer(typeof(Serializer))]
public class T3PositionSampleData : T3VertexSampleDatabase
{
    public new class Serializer : MetaSerializer<T3PositionSampleData>
    {
        public override void Serialize(ref T3PositionSampleData obj, MetaStream stream, MetaClassType? type = null)
        {
            T3VertexSampleDatabase objData = obj;
            stream.Serialize(ref objData);
            obj = (T3PositionSampleData)objData;
        }
    }
}

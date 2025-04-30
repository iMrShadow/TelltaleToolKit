using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;


/// <summary>
/// Old name - D3DVertexBuffer
/// </summary>
[MetaClassSerializerGlobal(typeof(T3VertexBufferSerializer))]
public class T3VertexBuffer
{
    [MetaMember("mNumVerts")]
    public int NumVerts { get; set; }

    [MetaMember("mVertFormat")]
    public uint VertFormat { get; set; }

    [MetaMember("mbLocked")]
    public bool Locked { get; set; }

    [MetaMember("mVertSize")]
    public int VertSize { get; set; }

    [MetaMember("mType")]
    public int Type { get; set; }

    [MetaMember("mbStoreCompressed")]
    public bool StoreCompressed { get; set; }

    public byte[] VertexBufferData = [];

    [MetaClassSerializerGlobal(typeof(T3VertexBufferSerializer))]
    public class T3VertexBufferSerializer : MetaClassSerializer<T3VertexBuffer>
    {
        private static readonly DefaultClassSerializer<T3VertexBuffer> DefaultSerializer = new();
        
        public override void Serialize(ref T3VertexBuffer obj, MetaStream stream)
        {
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter)
            {
                throw new NotImplementedException($"There is no serializer for {SerializationType}");
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int totalBytes = obj.StoreCompressed switch
                {
                    true when obj.Type is 2 or 4 => 2 * obj.NumVerts,
                    _ => obj.NumVerts * obj.VertSize
                };

                obj.VertexBufferData = streamReader.ReadBytes(totalBytes);
            }
        }
    }
}

// Right, so D3DVertexBuffer changes to T3VertexBuffer (probably because the engine became cross-platform at the time).
// Since most values are the same, I merged D3DVertexBuffer with T3VertexBuffer.

// [DataSerializerGlobal(typeof(DefaultClassSerializer<D3DVertexBuffer>))]
// public class D3DVertexBuffer
// {
//     [MetaMember("mNumVerts")] public int NumVerts { get; set; }
//     [MetaMember("mVertFormat")] public ulong VertFormat { get; set; }
//     [MetaMember("mVertSize")] public int VertSize { get; set; }
//     [MetaMember("mType")] public int Type { get; set; }
//     [MetaMember("mbStoreCompressed")] public bool StoreCompressed { get; set; }
//     public byte[] VertexBufferData = [];
//
//     [DataSerializerGlobal(typeof(D3DIndexBufferSerializer))]
//     public class D3DIndexBufferSerializer : MetaClassSerializer<D3DIndexBuffer>
//     {
//         public override void Serialize(ref D3DIndexBuffer obj, MetaStream stream, MetaClass desc)
//         {
//             new DefaultClassSerializer<D3DIndexBuffer>().Serialize(ref obj, stream, desc);
//
//             if (stream is MetaStreamWriter streamWriter)
//             {
//             }
//             else if (stream is MetaStreamReader streamReader)
//             {
//             }
//         }
//     }
// }
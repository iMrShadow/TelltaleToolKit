using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

/// <summary>
/// Old name - D3DIndexBuffer
/// </summary>
[MetaClassSerializerGlobal(typeof(T3IndexBufferSerializer))]
public class T3IndexBuffer
{
    [MetaMember("mbLocked")]
    public bool Locked { get; set; }

    [MetaMember("mFormat")]
    public int Format { get; set; }

    [MetaMember("mNumIndicies")]
    public int NumIndices { get; set; }

    [MetaMember("mIndexByteSize")]
    public int IndexByteSize { get; set; }

    public byte[] IndexBufferData = [];

    [MetaClassSerializerGlobal(typeof(T3IndexBufferSerializer))]
    public class T3IndexBufferSerializer : MetaClassSerializer<T3IndexBuffer>
    {
        private static readonly DefaultClassSerializer<T3IndexBuffer> DefaultSerializer = new();
        
        public override void Serialize(ref T3IndexBuffer obj, MetaStream stream)
        {
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                var bufferBytes = 2;
                if (obj.Format != 101)
                {
                    bufferBytes = 4;
                }

                obj.IndexBufferData = streamReader.ReadBytes(obj.IndexByteSize * obj.NumIndices);
            }
        }
    }
}

// Right, so D3DIndexBuffer changes to T3IndexBuffer (probably because the engine became cross-platform at the time).
// Since most values are the same, I merged D3DIndexBuffer with T3IndexBuffer.

// [DataSerializerGlobal(typeof(D3DIndexBufferSerializer))]
// public class D3DIndexBuffer
// {
//     [MetaMember("mbLocked")] public bool Locked { get; set; }
//     [MetaMember("mFormat")] public int Format { get; set; }
//     [MetaMember("mNumIndicies")] public int NumIndices { get; set; }
//     [MetaMember("mIndexByteSize")] public int IndexByteSize { get; set; }
//     public byte[] IndexBufferData = [];
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
//                 var bufferBytes = 2;
//                 if (obj.Format != 101)
//                 {
//                     bufferBytes = 4;
//                 }
//
//                 obj.IndexBufferData = streamReader.ReadBytes(obj.IndexByteSize * obj.NumIndices);
//             }
//         }
//     }
// }
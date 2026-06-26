using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

// Right, so D3DVertexBuffer changes to T3VertexBuffer (probably because the engine became cross-platform at the time).
// Since most values are the same, I merged D3DVertexBuffer with T3VertexBuffer.

/// <summary>
/// Old name - D3DVertexBuffer
/// </summary>
[MetaSerializer(typeof(Serializer))]
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

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mUsage")]
    public int Usage { get; set; }

    [MetaMember("mVertexComponents")]
    public T3VertexComponent[] VertexComponents { get; set; } = new T3VertexComponent[14];

    [MetaMember("mComponents")]
    public T3VertexComponent[] Components { get; set; } = new T3VertexComponent[12];

    [MetaMember("mbStoreCompressed")]
    public bool StoreCompressed { get; set; }

    public byte[] Buffer = [];

    public class Serializer : MetaSerializer<T3VertexBuffer>
    {
        private static readonly MetaClassSerializer<T3VertexBuffer> s_metaClassSerializer = new();

        public override void PreSerialize(ref T3VertexBuffer? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new T3VertexBuffer();
        }

        public override void Serialize(ref T3VertexBuffer obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj.Buffer.Length == obj.NumVerts * obj.VertSize)
                {
                    obj.StoreCompressed = false;
                }
                else if (obj.Buffer.Length == 2 * obj.NumVerts && obj.Type is 2 or 4)
                {
                    obj.StoreCompressed = true;
                }
            }

            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Buffer);
            }
            else
            {
                if (obj.Flags.Has(1))
                {
                    return;
                }

                int totalBytes = obj.StoreCompressed switch
                {
                    true when obj.Type is 2 or 4 => 2 * obj.NumVerts,
                    _ => obj.NumVerts * obj.VertSize
                };

                obj.Buffer = stream.ReadBytes(totalBytes);
            }
        }
    }
}

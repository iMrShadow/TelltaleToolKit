using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class T3GFXVertexState
{
    // TODO: Merge with T3MeshVertexState, old games use it.

    [MetaMember("mVertexCountPerInstance")]
    public uint VertexCountPerInstance { get; set; }

    [MetaMember("mIndexBufferCount")]
    public uint IndexBufferCount { get; set; }

    [MetaMember("mVertexBufferCount")]
    public uint VertexBufferCount { get; set; }

    [MetaMember("mAttributeCount")]
    public uint AttributeCount { get; set; }

    public List<GFXPlatformAttributeParams> Attributes { get; set; } = [];
    public List<T3GFXBuffer> IndexBuffer { get; set; } = [];
    public List<T3GFXBuffer> VertexBuffer { get; set; } = [];

    public class Serializer : MetaClassSerializer<T3GFXVertexState>
    {
        private static readonly DefaultClassSerializer<T3GFXVertexState> DefaultSerializer = new();

        public override void Serialize(ref T3GFXVertexState obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            MetaClass? desc = stream.GetMetaClass(typeof(T3GFXVertexState));

            if (stream is MetaStreamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if (obj.AttributeCount > 32)
                    throw new InvalidDataException("AttributeCount is too large");

                if (obj.VertexCountPerInstance > 32)
                    throw new InvalidDataException("VertexCountPerInstance is too large"); // Remove this for modding

                if (obj.IndexBufferCount > 4)
                    throw new InvalidDataException("IndexBufferCount is too large");

                for (var i = 0; i < obj.AttributeCount; i++)
                {
                    var attribute = new GFXPlatformAttributeParams();
                    TTK.PreSerialize(ref attribute, stream);
                    TTK.Serialize(ref attribute, stream);
                    obj.Attributes.Add(attribute);
                }

                if (desc is not null && desc.ContainsMember("mIndexBufferCount"))
                {
                    for (var i = 0; i < obj.IndexBufferCount; i++)
                    {
                        var indexBuffer = new T3GFXBuffer();
                        TTK.PreSerialize(ref indexBuffer, stream);
                        TTK.Serialize(ref indexBuffer, stream);
                        obj.IndexBuffer.Add(indexBuffer);
                    }
                }
                else
                {
                    obj.IndexBufferCount = 1;

                    bool hasIndexBuffer = streamReader.ReadBoolean();
                    if (hasIndexBuffer)
                    {
                        var indexBuffer = new T3GFXBuffer();
                        TTK.PreSerialize(ref indexBuffer, stream);
                        TTK.Serialize(ref indexBuffer, stream);
                        obj.IndexBuffer.Add(indexBuffer);
                    }
                }

                for (var i = 0; i < obj.VertexBufferCount; i++)
                {
                    var submesh = new T3GFXBuffer();
                    TTK.PreSerialize(ref submesh, stream);
                    TTK.Serialize(ref submesh, stream);
                    obj.VertexBuffer.Add(submesh);
                }
            }
        }
    }
}
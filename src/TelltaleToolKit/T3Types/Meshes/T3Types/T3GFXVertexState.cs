using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(Serializer))]
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

    public class Serializer : MetaSerializer<T3GFXVertexState>
    {
        private static readonly MetaClassSerializer<T3GFXVertexState> s_metaClassSerializer = new();

        public override void Serialize(ref T3GFXVertexState obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            MetaClass? desc = stream.GetMetaClass(typeof(T3GFXVertexState));

            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj.AttributeCount > 32)
                    throw new InvalidDataException("AttributeCount is too large");

                /*
                 if (obj.VertexCountPerInstance > 32)
                  throw new InvalidDataException("VertexCountPerInstance is too large");
                */

                if (desc is not null && desc.ContainsMember("mIndexBufferCount") && obj.IndexBufferCount > 4)
                    throw new InvalidDataException("IndexBufferCount is too large");

                for (var i = 0; i < obj.Attributes.Count; i++)
                {
                    GFXPlatformAttributeParams attribute = obj.Attributes[i];
                    stream.Serialize(ref attribute);
                    obj.Attributes[i] = attribute;
                }

                if (desc is not null && desc.ContainsMember("mIndexBufferCount"))
                {
                    for (var i = 0; i < obj.IndexBuffer.Count; i++)
                    {
                        T3GFXBuffer indexBuffer = obj.IndexBuffer[i];
                        stream.Serialize(ref indexBuffer);
                        obj.IndexBuffer[i] = indexBuffer;
                    }
                }
                else
                {
                    bool hasIndexBuffer = obj.IndexBuffer.Count > 0;
                    stream.Write(hasIndexBuffer);

                    if (hasIndexBuffer)
                    {
                        T3GFXBuffer indexBuffer = obj.IndexBuffer[0];
                        stream.Serialize(ref indexBuffer);
                        obj.IndexBuffer[0] = indexBuffer;
                    }
                }

                for (var i = 0; i < obj.VertexBuffer.Count; i++)
                {
                    T3GFXBuffer vertexBuffer = obj.VertexBuffer[i];
                    stream.Serialize(ref vertexBuffer);
                    obj.VertexBuffer[i] = vertexBuffer;
                }
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                if (obj.AttributeCount > 32)
                    throw new InvalidDataException("AttributeCount is too large");

                //if (obj.VertexCountPerInstance > 32)
                //throw new InvalidDataException("VertexCountPerInstance is too large"); // Remove this for modding

                if (obj.IndexBufferCount > 4)
                    throw new InvalidDataException("IndexBufferCount is too large");

                for (var i = 0; i < obj.AttributeCount; i++)
                {
                    var attribute = new GFXPlatformAttributeParams();
                    stream.Serialize(ref attribute);
                    obj.Attributes.Add(attribute);
                }

                if (desc is not null && desc.ContainsMember("mIndexBufferCount"))
                {
                    for (var i = 0; i < obj.IndexBufferCount; i++)
                    {
                        var indexBuffer = new T3GFXBuffer();
                        stream.Serialize(ref indexBuffer);
                        obj.IndexBuffer.Add(indexBuffer);
                    }
                }
                else
                {
                    obj.IndexBufferCount = 1;

                    bool hasIndexBuffer = stream.ReadBoolean();
                    if (hasIndexBuffer)
                    {
                        var indexBuffer = new T3GFXBuffer();
                        stream.Serialize(ref indexBuffer);
                        obj.IndexBuffer.Add(indexBuffer);
                    }
                }

                for (var i = 0; i < obj.VertexBufferCount; i++)
                {
                    var submesh = new T3GFXBuffer();
                    stream.Serialize(ref submesh);
                    obj.VertexBuffer.Add(submesh);
                }
            }
        }
    }
}

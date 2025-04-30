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

    public GFXPlatformAttributeParams[] Attributes = new GFXPlatformAttributeParams[32];
    public T3GFXBuffer[] IndexBuffer = new T3GFXBuffer[4];
    public T3GFXBuffer[] VertexBuffer = new T3GFXBuffer[32];

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
                
                if(obj.VertexCountPerInstance > 32)
                    throw new InvalidDataException("VertexCountPerInstance is too large"); // Remove this for modding
                
                if(obj.IndexBufferCount > 4)
                    throw new InvalidDataException("IndexBufferCount is too large");

                if (obj.AttributeCount > 0)
                {
                    for (var i = 0; i < obj.AttributeCount; i++)
                    {
                        TTK.PreSerialize(ref obj.Attributes[i], stream);
                        TTK.Serialize(ref obj.Attributes[i], stream);
                    }
                }
                
                if (desc is not null && desc.ContainsMember("mIndexBufferCount"))
                {
                    for (var i = 0; i < obj.IndexBufferCount; i++)
                    {
                        TTK.PreSerialize(ref obj.IndexBuffer[i], stream);
                        TTK.Serialize(ref obj.IndexBuffer[i], stream);
                    }
                }
                else
                {
                    obj.IndexBufferCount = 1;

                    bool hasIndexBuffer = streamReader.ReadBoolean();
                    if (hasIndexBuffer)
                    {
                        TTK.PreSerialize(ref obj.IndexBuffer[0], stream);
                        TTK.Serialize(ref obj.IndexBuffer[0], stream);
                    }
                }

                for (var i = 0; i < obj.VertexBufferCount; i++)
                {
                    TTK.PreSerialize(ref obj.VertexBuffer[i], stream);
                    TTK.Serialize(ref obj.VertexBuffer[i], stream);
                }
            }
        }
    }
}
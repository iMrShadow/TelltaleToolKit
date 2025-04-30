using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class T3GFXBuffer
{
    // TODO: Merge with T3MeshBuffer

    [MetaMember("mResourceUsage")]
    public GFXPlatformResourceUsage ResourceUsage { get; set; }

    [MetaMember("mBufferFormat")]
    public GFXPlatformFormat BufferFormat { get; set; }

    // [MetaMember("mBufferUsage")] This fails, because there's no flag set for this.
    // public GFXPlatformBufferUsage BufferUsage { get; set; }  
    
    [MetaMember("mBufferUsage")]
    public uint BufferUsage { get; set; }

    [MetaMember("mCount")]
    public uint Count { get; set; }

    [MetaMember("mStride")]
    public uint Stride { get; set; }

    // cpu buffer
    public byte[] Buffer = [];

    public class Serializer : MetaClassSerializer<T3GFXBuffer>
    {
        private static readonly DefaultClassSerializer<T3GFXBuffer> DefaultSerializer = new();

        public override void Serialize(ref T3GFXBuffer obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            // I have a slightly different implementation
            // Telltale's meta serializer only applies to write-only streams
            // In read mode, they read the bytes at the end in the T3GFXVertexState
            // I assume it's done like for better performance
            // This requires further testing
            stream.BeginAsyncSection();
            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.Buffer);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.Buffer = streamReader.ReadBytes((int)(obj.Count * obj.Stride));
            }

            stream.EndAsyncSection();
        }
    }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<GFXPlatformResourceUsage>))]
public enum GFXPlatformResourceUsage
{
    //  eGFXPlatformUsage_
    Immutable = 0x0,
    Dynamic = 0x1,
    Streaming = 0x2,
    DynamicUnsynchronized = 0x3,
    GPUWritable = 0x4,
    CPUReadStaging = 0x5,
    CPUWriteStaging = 0x6,
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<GFXPlatformBufferUsage>))]
public enum GFXPlatformBufferUsage
{
    //  eGFXPlatformBuffer_
    None = 0,
    Vertex = 1,
    Index = 2,
    Uniform = 4,
    ShaderRead = 8,
    ShaderWrite = 0x10,
    ShaderReadWrite = 0x18,
    ShaderRawAccess = 0x20,
    ShaderReadRaw = 0x28,
    ShaderWriteRaw = 0x30,
    ShaderReadWriteRaw = 0x38,
    DrawIndirectArgs = 0x40,
    SingleValue = 0x80,
}
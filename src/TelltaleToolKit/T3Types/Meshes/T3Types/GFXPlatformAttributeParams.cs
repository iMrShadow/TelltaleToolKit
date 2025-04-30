using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<GFXPlatformAttributeParams>))]
public class GFXPlatformAttributeParams
{
    [MetaMember("mAttribute")]
    public GFXPlatformVertexAttribute Attribute { get; set; }

    [MetaMember("mFormat")]
    public GFXPlatformFormat Format { get; set; }

    [MetaMember("mFrequency")]
    public GFXPlatformVertexFrequency Frequency { get; set; }

    [MetaMember("mAttributeIndex")]
    public uint AttributeIndex { get; set; }

    [MetaMember("mBufferOffset")]
    public uint BufferOffset { get; set; }

    [MetaMember("mBufferIndex")]
    public uint BufferIndex { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<GFXPlatformVertexFrequency>))]
public enum GFXPlatformVertexFrequency
{
    //  eGFXPlatformFrequency_
    PerVertex = 0x0,
    PerInstance = 0x1,
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<GFXPlatformVertexAttribute>))]
public enum GFXPlatformVertexAttribute
{
    //   eGFXPlatformAttribute_

    Position = 0,
    Normal = 1,
    Tangent = 2,
    BlendWeight = 3,
    BlendIndex = 4,
    Color = 5,
    TexCoord = 6,
}
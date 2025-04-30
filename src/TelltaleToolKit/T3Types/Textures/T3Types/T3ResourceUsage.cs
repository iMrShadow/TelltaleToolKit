using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3ResourceUsage>))]
public enum T3ResourceUsage
{
    // eResourceUsage
    Static = 0,
    Dynamic = 1,
    System = 2,
    RenderTarget = 3,
    ShaderWrite = 4,
}
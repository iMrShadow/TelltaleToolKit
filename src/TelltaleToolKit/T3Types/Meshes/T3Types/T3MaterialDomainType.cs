using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(EnumSerializer<T3MaterialDomainType>))]
public enum T3MaterialDomainType
{
    None = -1,
    Mesh = 0x0,
    Particle = 0x1,
    Decal = 0x2,
    Post = 0x3,
    ExportMeshShader = 0x4,
    Count = 0x5,
}

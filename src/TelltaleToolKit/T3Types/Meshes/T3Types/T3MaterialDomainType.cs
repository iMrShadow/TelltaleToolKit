using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialDomainType>))]
public enum T3MaterialDomainType
{
    // eMaterialDomain_  
    None = -1,
    Mesh = 0x0,
    Particle = 0x1,
    Decal = 0x2,
    Post = 0x3,
    ExportMeshShader = 0x4,
    Count = 0x5,
}
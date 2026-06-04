using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumMeshDebugRenderType>))]
public struct EnumMeshDebugRenderType
{
    [MetaMember("mVal")]
    public MeshDebugRenderType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<MeshDebugRenderType>))]
    public enum MeshDebugRenderType
    {
        eMeshRender_Solid = 1,
        eMeshRender_Wireframe = 2
    }
}

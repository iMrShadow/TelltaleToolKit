using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumMeshDebugRenderType>))]
public struct EnumMeshDebugRenderType
{
    [MetaMember("mVal")]
    public MeshDebugRenderType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<MeshDebugRenderType>))]
public enum MeshDebugRenderType
{
    // TODO:
}

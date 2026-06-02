using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(EnumSerializer<T3MeshEndianType>))]
public enum T3MeshEndianType
{
    // eMeshEndian_
    Default = 0,
    Swap = 1,
    SwapBytesAsWord = 2
}

using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MeshTexCoordTransform>))]
public class T3MeshTexCoordTransform
{
    [MetaMember("mScale")]
    public Vector2 Scale { get; set; } = Vector2.One;

    [MetaMember("mOffset")]
    public Vector2 Offset { get; set; } = Vector2.Zero;
}

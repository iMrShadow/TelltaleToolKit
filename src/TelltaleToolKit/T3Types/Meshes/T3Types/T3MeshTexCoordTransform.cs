using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshTexCoordTransform>))]
public class T3MeshTexCoordTransform
{
    [MetaMember("mScale")]
    public Vector2 Scale { get; set; } = Vector2.One;
    
    [MetaMember("mOffset")]
    public Vector2 Offset { get; set; } = Vector2.Zero;
}
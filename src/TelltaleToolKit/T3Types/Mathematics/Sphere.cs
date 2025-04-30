using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Sphere>))]
public struct Sphere
{
    [MetaMember("mCenter")]
    public Vector3 Center { get; set; }

    [MetaMember("mRadius")]
    public float Radius { get; set; }
}
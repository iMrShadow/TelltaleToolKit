using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaSerializer(typeof(MetaClassSerializer<Sphere>))]
public struct Sphere
{
    [MetaMember("mCenter")]
    public Vector3 Center { get; set; }

    [MetaMember("mRadius")]
    public float Radius { get; set; }

    public override string ToString() => $"{Center}, {Radius}";
}

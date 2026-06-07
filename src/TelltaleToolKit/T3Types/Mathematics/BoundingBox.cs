using System.Numerics;
using System.Runtime.InteropServices;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaSerializer(typeof(MetaClassSerializer<BoundingBox>))]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct BoundingBox
{
    [MetaMember("mMin")]
    public Vector3 Min { get; set; }

    [MetaMember("mMax")]
    public Vector3 Max { get; set; }

    public override string ToString() => $"{Min}, {Max}";
}

using System.Runtime.InteropServices;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<BoundingBox>))]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct BoundingBox
{
    [MetaMember("mMin")] public Vector3 Min {get; set;}

    [MetaMember("mMax")] public Vector3 Max{get; set;}

    public override string ToString() => $"{Min}, {Max}";
}
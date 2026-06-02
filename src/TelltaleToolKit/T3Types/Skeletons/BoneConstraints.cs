using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<BoneConstraints>))]
public class BoneConstraints
{
    [MetaMember("mBoneType")]
    public BoneType BoneType { get; set; }

    [MetaMember("mHingeAxis")]
    public Vector3 HingeAxis { get; set; }

    [MetaMember("mAxisRange")]
    public Range<float>[] AxisRange { get; set; } = [new(), new(), new()];
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<BoneType>))]
public enum BoneType
{
    // eBoneType_
    Hinge = 0,
    Ball = 1
}

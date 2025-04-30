using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
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
    public Range<float>[] AxisRange { get; set; } = new Range<float>[3];
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<BoneType>))]
public enum BoneType
{
    // eBoneType_
    Hinge = 0,
    Ball = 1
};
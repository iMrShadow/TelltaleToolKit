using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

// .look Files
[MetaSerializer(typeof(MetaClassSerializer<ProceduralLookAt>))]
public class ProceduralLookAt
{
    [MetaMember("Baseclass_Animation")]
    public Animation Animation { get; set; } = new();

    [MetaMember("mHostNode")]
    public string HostNode { get; set; } = string.Empty;

    [MetaMember("mTargetAgent")]
    public string TargetAgent { get; set; } = string.Empty;

    [MetaMember("mTargetNode")]
    public string TargetNode { get; set; } = string.Empty;

    [MetaMember("mTargetOffset")]
    public Vector3 TargetOffset { get; set; }

    [MetaMember("mbUsePrivateNode")]
    public bool UsePrivateNode { get; set; }

    [MetaMember("mhXAxisChore")]
    public AnimOrChore XAxisChore { get; set; } = new();

    [MetaMember("mhYAxisChore")]
    public AnimOrChore YAxisChore { get; set; } = new();

    [MetaMember("mbRotateHostNode")]
    public bool RotateHostNode { get; set; }

    [MetaMember("mLastLRAngle")]
    public float LastLrAngle { get; set; }

    [MetaMember("mLastUDAngle")]
    public float LastUdAngle { get; set; }

    [MetaMember("mLastLRWeight")]
    public float LastLrWeight { get; set; }

    [MetaMember("mLastUDWeight")]
    public float LastUdWeight { get; set; }

    [MetaMember("mLookAtComputeStage")]
    public EnumLookAtComputeStage LookAtComputeStageE { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<EnumLookAtComputeStage>))]
    public struct EnumLookAtComputeStage
    {
        [MetaMember("mVal")]
        public LookAtComputeStage Val { get; set; }
    }

    [MetaSerializer(typeof(EnumSerializer<LookAtComputeStage>))]
    public enum LookAtComputeStage
    {
        IdleLookAt = 0,
        DialogChoreLookAt = 1,
        FinalLookAt = 2
    }

    [MetaSerializer(typeof(MetaClassSerializer<Constraint>))]
    public struct Constraint
    {
        [MetaMember("mMaxLeftRight")]
        public float MaxLeftRight { get; set; }

        [MetaMember("mMinLeftRight")]
        public float MinLeftRight { get; set; }

        [MetaMember("mMaxUp")]
        public float MaxUp { get; set; }

        [MetaMember("mMinUp")]
        public float MinUp { get; set; }

        [MetaMember("mLeftRightFixedOffsset")]
        public float LeftRightFixedOffset { get; set; }

        [MetaMember("mUpDownFixedOffsset")]
        public float UpDownFixedOffset { get; set; }
    }

    // public class Serializer : MetaSerializer<ProceduralLookAt>
    // {
    //     public override void Serialize(ref ProceduralLookAt obj, MetaStream stream, MetaClassType? type = null)
    //     {
    //         if (stream.Mode is MetaStreamMode.Write)
    //         {
    //
    //         }
    //         else if (stream.Mode is MetaStreamMode.Read)
    //         {
    //
    //         }
    //     }
    // }
}

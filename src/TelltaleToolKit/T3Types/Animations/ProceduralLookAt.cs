using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Animations;

// TODO: .look Files
[MetaClassSerializerGlobal(typeof(Serializer))]
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
    public EnumLookAtComputeStage LookAtComputeStage { get; set; }

    public struct EnumLookAtComputeStage
    {
        [MetaMember("mVal")]
        public int Val { get; set; }
        // 0 = idle look at
        // 1 = dialog chore look at
        // 2 = final look at
    }

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

    public class Serializer : MetaClassSerializer<ProceduralLookAt>
    {
        public override void Serialize(ref ProceduralLookAt obj, MetaStream stream)
        {
            // TODO: Check with meta stream version.
            if (stream is MetaStreamWriter)
            {
                if (stream.Configuration.Version is MetaStreamVersion.Msv5 or MetaStreamVersion.Msv6)
                {
                    return;
                }

                Animation objAnimation = obj.Animation;
                TTKContext.Instance().GetSerializer<Animation>().Serialize(ref objAnimation, stream);
            }
            else if (stream is MetaStreamReader)
            {
                if (stream.Configuration.Version is MetaStreamVersion.Msv5 or MetaStreamVersion.Msv6)
                {
                    return;
                }

                var animation = new Animation();
                TTKContext.Instance().GetSerializer<Animation>().Serialize(ref animation, stream);

                obj.Animation = animation;
            }
        }
    }
};
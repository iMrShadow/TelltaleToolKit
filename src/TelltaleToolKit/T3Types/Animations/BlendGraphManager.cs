using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

// .BGM
[MetaSerializer(typeof(MetaClassSerializer<BlendGraphManager>))]
public class BlendGraphManager
{
    [MetaMember( "mfTransitionTime")]
    public float TransitionTime { get; set; }
    [MetaMember( "mIdleAnimOrChore")]
    public AnimOrChore IdleAnimOrChore { get; set; }
    [MetaMember( "mbUseAnimationMoverData")]
    public bool UseAnimationMoverData { get; set; }
    [MetaMember( "mhFreewalkStartGraph")]
    public Handle<BlendGraph> FreewalkStartGraph { get; set; }
    [MetaMember( "mhFreewalkLoopGraph")]
    public Handle<BlendGraph> FreewalkLoopGraph { get; set; }
    [MetaMember( "mhFreewalkStopGraph")]
    public Handle<BlendGraph> FreewalkStopGraph { get; set; }
    [MetaMember( "mhTurnToFaceGraph")]
    public Handle<BlendGraph> TurnToFaceGraph { get; set; }
    [MetaMember( "mhChoredMovementStartGraph")]
    public Handle<BlendGraph> ChoredMovementStartGraph { get; set; }
    [MetaMember( "mhChoredMovementLoopGraph")]
    public Handle<BlendGraph> ChoredMovementLoopGraph { get; set; }
    [MetaMember( "mhChoredMovementStopGraph")]
    public Handle<BlendGraph> ChoredMovementStopGraph { get; set; }
    [MetaMember( "mVersion")]
    public int Version { get; set; }
    [MetaMember( "mbUseAlgorithmicHeadTurn")]
    public bool UseAlgorithmicHeadTurn { get; set; }
    [MetaMember( "mfMaxManualSteeringVelocityInDegrees")]
    public float MaxManualSteeringVelocityInDegrees { get; set; }
    [MetaMember( "mfMinManualSteeringVelocityInDegrees")]
    public float MinManualSteeringVelocityInDegrees { get; set; }
    [MetaMember( "mfMaxLeanInPercentVelocity")]
    public float MaxLeanInPercentVelocity { get; set; }
    [MetaMember( "mfMinLeanInPercentVelocity")]
    public float MinLeanInPercentVelocity { get; set; }
    [MetaMember( "mfWalkSpeedScale")]
    public float WalkSpeedScale { get; set; }
    [MetaMember( "mfRunSpeedScale")]
    public float RunSpeedScale { get; set; }
}

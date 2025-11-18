using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PlaceableBallTwistJointKey>))]
public class PlaceableBallTwistJointKey
{
    [MetaMember("mBoneLengthConstraint__Enabled")]
    public bool mBoneLengthConstraint__Enabled { get; set; }

    [MetaMember("mBoneLengthConstraint_RestRelative")]
    public bool mBoneLengthConstraint_RestRelative { get; set; }

    [MetaMember("mBoneLengthConstraint_NodeMobility")]
    public float mBoneLengthConstraint_NodeMobility { get; set; }

    [MetaMember("mBoneLengthConstraint_ParentMobility")]
    public float mBoneLengthConstraint_ParentMobility { get; set; }

    [MetaMember("mAngleConstraint__Enabled")]
    public bool mAngleConstraint__Enabled { get; set; }

    [MetaMember("mAngleConstraint_MaxBendAngle")]
    public float mAngleConstraint_MaxBendAngle { get; set; }

    [MetaMember("mAngleConstraint_MinTwistAngle")]
    public float mAngleConstraint_MinTwistAngle { get; set; }

    [MetaMember("mAngleConstraint_MaxTwistAngle")]
    public float mAngleConstraint_MaxTwistAngle { get; set; }

    [MetaMember("mPlacementLockToAnimConstraint__Enabled")]
    public bool mPlacementLockToAnimConstraint__Enabled { get; set; } 
    
    [MetaMember("mPlacementLockToAnimConstraint_Offset")]
    public Transform mPlacementLockToAnimConstraint_Offset { get; set; }  
    
    [MetaMember("mPlacementLockToNodeConstraint_Offset")]
    public Transform mPlacementLockToNodeConstraint_Offset { get; set; }

    [MetaMember("mPlacementLockToAnimConstraint_LockMode")]
    public AnimationConstraint.LockMode mPlacementLockToAnimConstraint_LockMode { get; set; }

    [MetaMember("mPlacementLockToNodeConstraint__Enabled")]
    public bool mPlacementLockToNodeConstraint__Enabled { get; set; }

    [MetaMember("mPlacementLockToNodeConstraint_Node")]
    public AnimationConstraint.Node mPlacementLockToNodeConstraint_Node { get; set; }

    [MetaMember("mPlacementLockToNodeConstraint_LockMode")]
    public AnimationConstraint.LockMode mPlacementLockToNodeConstraint_LockMode { get; set; }

    [MetaMember("mTranslationConstraint__Enabled")]
    public bool mTranslationConstraint__Enabled { get; set; }

    [MetaMember("mTranslationConstraint_ReferenceFrame")]
    public ConstraintReferenceFrame mTranslationConstraint_ReferenceFrame { get; set; }

    [MetaMember("mTranslationConstraint_MaxVelocity")]
    public float mTranslationConstraint_MaxVelocity { get; set; }

    [MetaMember("mTranslationConstraint_MaxAcceleration")]
    public float mTranslationConstraint_MaxAcceleration { get; set; }

    [MetaMember("mRotationConstraint__Enabled")]
    public bool mRotationConstraint__Enabled { get; set; }

    [MetaMember("mRotationConstraint_ReferenceFrame")]
    public ConstraintReferenceFrame mRotationConstraint_ReferenceFrame { get; set; }

    [MetaMember("mRotationConstraint_MaxBendAngularVelocity")]
    public float mRotationConstraint_MaxBendAngularVelocity { get; set; }

    [MetaMember("mRotationConstraint_MaxBendAngularAcceleration")]
    public float mRotationConstraint_MaxBendAngularAcceleration { get; set; }

    [MetaMember("mRotationConstraint_MaxTwistAngularVelocity")]
    public float mRotationConstraint_MaxTwistAngularVelocity { get; set; }

    [MetaMember("mRotationConstraint_MaxTwistAngularAcceleration")]
    public float mRotationConstraint_MaxTwistAngularAcceleration { get; set; }
}

public struct AnimationConstraint
{
    [MetaClassSerializerGlobal(typeof(EnumSerializer<LockMode>))]
    public enum LockMode
    {
        eTrafoMode = 0, /*pos and rot*/
        ePosMode = 1, /*pos*/
        eOriMode = 2, /*rot*/
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<Node>))]
    public enum Node
    {
        eWorld = 0x0,
        eRoot = 0x1,
        eSpine1 = 0x2,
        eSpine2 = 0x3,
        eSpine3 = 0x4,
        eSpine4 = 0x5,
        eShoulder_L = 0x6,
        eArm_L = 0x7,
        eElbow_L = 0x8,
        eWrist_L = 0x9,
        eMiddleF1_L = 0xA,
        eMiddleF2_L = 0xB,
        eMiddleF3_L = 0xC,
        eIndexF1_L = 0xD,
        eIndexF2_L = 0xE,
        eIndexF3_L = 0xF,
        eThumb1_L = 0x10,
        eThumb2_L = 0x11,
        eThumb3_L = 0x12,
        eRingF1_L = 0x13,
        eRingF2_L = 0x14,
        eRingF3_L = 0x15,
        ePinkyF1_L = 0x16,
        ePinkyF2_L = 0x17,
        ePinkyF3_L = 0x18,
        eShoulder_R = 0x19,
        eArm_R = 0x1A,
        eElbow_R = 0x1B,
        eWrist_R = 0x1C,
        ePinkyF1_R = 0x1D,
        ePinkyF2_R = 0x1E,
        ePinkyF3_R = 0x1F,
        eMiddleF1_R = 0x20,
        eMiddleF2_R = 0x21,
        eMiddleF3_R = 0x22,
        eIndexF1_R = 0x23,
        eIndexF2_R = 0x24,
        eIndexF3_R = 0x25,
        eThumb1_R = 0x26,
        eThumb2_R = 0x27,
        eThumb3_R = 0x28,
        eRingF1_R = 0x29,
        eRingF2_R = 0x2A,
        eRingF3_R = 0x2B,
        eNeck = 0x2C,
        eNeck2 = 0x2D,
        eHead = 0x2E,
        eEye_L = 0x2F,
        eEye_R = 0x30,
        ePelvis = 0x31,
        eLeg_L = 0x32,
        eKnee_L = 0x33,
        eAnkle_L = 0x34,
        eLeg_R = 0x35,
        eKnee_R = 0x36,
        eAnkle_R = 0x37,
        eNumNodes = 0x38,
    };
}
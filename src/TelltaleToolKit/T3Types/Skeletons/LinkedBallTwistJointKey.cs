using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LinkedBallTwistJointKey>))]
public class LinkedBallTwistJointKey
{
    [MetaMember("mBoneLengthConstraint__Enabled")]
    public bool mBoneLengthConstraint__Enabled{get;set;}

    [MetaMember("mBoneLengthConstraint_RestRelative")]
    public bool mBoneLengthConstraint_RestRelative{get;set;}

    [MetaMember("mBoneLengthConstraint_NodeMobility")]
    public float mBoneLengthConstraint_NodeMobility{get;set;}

    [MetaMember("mBoneLengthConstraint_ParentMobility")]
    public float mBoneLengthConstraint_ParentMobility{get;set;}

    [MetaMember("mAngleConstraint__Enabled")]
    public bool mAngleConstraint__Enabled{get;set;}

    [MetaMember("mAngleConstraint_MaxBendAngle")]
    public float mAngleConstraint_MaxBendAngle{get;set;}

    [MetaMember("mAngleConstraint_MinTwistAngle")]
    public float mAngleConstraint_MinTwistAngle{get;set;}

    [MetaMember("mAngleConstraint_MaxTwistAngle")]
    public float mAngleConstraint_MaxTwistAngle{get;set;}

    [MetaMember("mTranslationConstraint__Enabled")]
    public bool mTranslationConstraint__Enabled{get;set;}

    [MetaMember("mTranslationConstraint_ReferenceFrame")]
    public ConstraintReferenceFrame mTranslationConstraint_ReferenceFrame{get;set;}

    [MetaMember("mTranslationConstraint_MaxVelocity")]
    public float mTranslationConstraint_MaxVelocity{get;set;}

    [MetaMember("mTranslationConstraint_MaxAcceleration")]
    public float mTranslationConstraint_MaxAcceleration{get;set;}

    [MetaMember("mRotationConstraint__Enabled")]
    public bool mRotationConstraint__Enabled{get;set;}

    [MetaMember("mRotationConstraint_ReferenceFrame")]
    public ConstraintReferenceFrame mRotationConstraint_ReferenceFrame{get;set;}

    [MetaMember("mRotationConstraint_MaxBendAngularVelocity")]
    public float mRotationConstraint_MaxBendAngularVelocity{get;set;}

    [MetaMember("mRotationConstraint_MaxBendAngularAcceleration")]
    public float mRotationConstraint_MaxBendAngularAcceleration{get;set;}

    [MetaMember("mRotationConstraint_MaxTwistAngularVelocity")]
    public float mRotationConstraint_MaxTwistAngularVelocity{get;set;}

    [MetaMember("mRotationConstraint_MaxTwistAngularAcceleration")]
    public float mRotationConstraint_MaxTwistAngularAcceleration{get;set;}

    [MetaMember("mLinkBendConstraint__Enabled")]
    public bool mLinkBendConstraint__Enabled{get;set;}

    [MetaMember("mLinkBendConstraint_Strength")]
    public float mLinkBendConstraint_Strength{get;set;}

    [MetaMember("mLinkBendConstraint_EchoDelay")]
    public float mLinkBendConstraint_EchoDelay{get;set;}

    [MetaMember("mLinkTwistConstraint__Enabled")]
    public bool mLinkTwistConstraint__Enabled{get;set;}

    [MetaMember("mLinkTwistConstraint_Strength")]
    public float mLinkTwistConstraint_Strength{get;set;}

    [MetaMember("mLinkTwistConstraint_EchoDelay")]
    public float mLinkTwistConstraint_EchoDelay{get;set;}
}
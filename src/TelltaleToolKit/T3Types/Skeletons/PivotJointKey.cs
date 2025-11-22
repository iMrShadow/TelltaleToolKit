using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PivotJointKey>))]
public class PivotJointKey
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

    [MetaMember("mAngleConstraint_MinHorizontalBendAngle")]
    public float mAngleConstraint_MinHorizontalBendAngle { get; set; }

    [MetaMember("mAngleConstraint_MaxHorizontalBendAngle")]
    public float mAngleConstraint_MaxHorizontalBendAngle { get; set; }

    [MetaMember("mAngleConstraint_MinVerticalBendAngle")]
    public float mAngleConstraint_MinVerticalBendAngle { get; set; }

    [MetaMember("mAngleConstraint_MaxVerticalBendAngle")]
    public float mAngleConstraint_MaxVerticalBendAngle { get; set; }

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
}
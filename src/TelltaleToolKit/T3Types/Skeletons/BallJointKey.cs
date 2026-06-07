using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaSerializer(typeof(MetaClassSerializer<BallJointKey>))]
public class BallJointKey
{
    [MetaMember("mBoneLengthConstraint__Enabled")]
    public bool BoneLengthConstraintEnabled { get; set; }

    [MetaMember("mBoneLengthConstraint_RestRelative")]
    public bool BoneLengthConstraintRestRelative { get; set; }

    [MetaMember("mBoneLengthConstraint_NodeMobility")]
    public float BoneLengthConstraintNodeMobility { get; set; }

    [MetaMember("mBoneLengthConstraint_ParentMobility")]
    public float BoneLengthConstraintParentMobility { get; set; }

    [MetaMember("mAngleConstraint__Enabled")]
    public bool AngleConstraintEnabled { get; set; }

    [MetaMember("mAngleConstraint_MaxBendAngle")]
    public float AngleConstraintMaxBendAngle { get; set; }

    [MetaMember("mTranslationConstraint__Enabled")]
    public bool TranslationConstraintEnabled { get; set; }

    [MetaMember("mTranslationConstraint_ReferenceFrame")]
    public ConstraintReferenceFrame TranslationConstraintReferenceFrame { get; set; }

    [MetaMember("mTranslationConstraint_MaxVelocity")]
    public float TranslationConstraintMaxVelocity { get; set; }

    [MetaMember("mTranslationConstraint_MaxAcceleration")]
    public float TranslationConstraintMaxAcceleration { get; set; }

    [MetaMember("mRotationConstraint__Enabled")]
    public bool RotationConstraintEnabled { get; set; }

    [MetaMember("mRotationConstraint_ReferenceFrame")]
    public ConstraintReferenceFrame RotationConstraintReferenceFrame { get; set; }

    [MetaMember("mRotationConstraint_MaxBendAngularVelocity")]
    public float RotationConstraintMaxBendAngularVelocity { get; set; }

    [MetaMember("mRotationConstraint_MaxBendAngularAcceleration")]
    public float RotationConstraintMaxBendAngularAcceleration { get; set; }
}

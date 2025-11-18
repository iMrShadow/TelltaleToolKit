using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<RootKey>))]
public class RootKey
{
    [MetaMember("mTranslationConstraint__Enabled")]
    public bool mTranslationConstraint__Enabled { get; set; }

    [MetaMember("mTranslationConstraint_MaxVelocity")]
    public float mTranslationConstraint_MaxVelocity { get; set; }

    [MetaMember("mTranslationConstraint_MaxAcceleration")]
    public float mTranslationConstraint_MaxAcceleration { get; set; }

    [MetaMember("mRotationConstraint__Enabled")]
    public bool mRotationConstraint__Enabled { get; set; }

    [MetaMember("mRotationConstraint_MaxBendAngularVelocity")]
    public float mRotationConstraint_MaxBendAngularVelocity { get; set; }

    [MetaMember("mRotationConstraint_MaxBendAngularAcceleration")]
    public float mRotationConstraint_MaxBendAngularAcceleration { get; set; }

    [MetaMember("mRotationConstraint_MaxTwistAngularVelocity")]
    public float mRotationConstraint_MaxTwistAngularVelocity { get; set; }

    [MetaMember("mRotationConstraint_MaxTwistAngularAcceleration")]
    public float mRotationConstraint_MaxTwistAngularAcceleration { get; set; }
}
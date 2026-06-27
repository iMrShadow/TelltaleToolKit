using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<PhysicsObject>))]
public class PhysicsObject
{
    [MetaSerializer(typeof(EnumSerializer<PhysicsBoundingVolumeType>))]
    public enum PhysicsBoundingVolumeType
    {
        Cylinder = 0,
        Box = 1,
        Sphere = 2
    }

    [MetaSerializer(typeof(EnumSerializer<PhysicsCollisionType>))]
    public enum PhysicsCollisionType
    {
        InterAgent = 0,
        Raycast = 1
    }

    [MetaMember("mbEnabledPropertyOn")]
    public bool EnabledPropertyOn { get; set; }

    [MetaMember("mCollisionType")]
    public PhysicsCollisionType CollisionType { get; set; }

    [MetaMember("mBoundingVolumeType")]
    public PhysicsBoundingVolumeType BoundingVolumeType { get; set; }

    [MetaMember("mfBoundingVolumeScalingFactor")]
    public float BoundingVolumeScalingFactor { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<EnumePhysicsBoundingVolumeType>))]
    public struct EnumePhysicsBoundingVolumeType
    {
        [MetaMember("mVal")]
        public PhysicsBoundingVolumeType Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumePhysicsCollisionType>))]
    public struct EnumePhysicsCollisionType
    {
        [MetaMember("mVal")]
        public PhysicsCollisionType Val { get; set; }
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Particles;

// .t3bullet
[MetaSerializer(typeof(MetaClassSerializer<PhysicsData>))]
public class PhysicsData
{
    [MetaMember("mDataBuffer")]
    public BinaryBuffer DataBuffer { get; set; } = new();
}

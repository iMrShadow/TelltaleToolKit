using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Transform>))]
public class Transform
{
    [MetaMember("mRot")]
    public Quaternion Rotation { get; set; } = new();

    [MetaMember("mTrans")]
    public Vector3 Translation { get; set; } = new();
}

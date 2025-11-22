using System.Numerics;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Transform>))]
public class Transform
{
    [MetaMember("mRot")]
    public Quaternion Rotation { get; set; } = new();

    [MetaMember("mTrans")]
    public Vector3 Translation { get; set; } = new();
}
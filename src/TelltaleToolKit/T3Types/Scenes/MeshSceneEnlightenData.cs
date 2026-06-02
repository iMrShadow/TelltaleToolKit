using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Scenes;

[MetaSerializer(typeof(MetaClassSerializer<MeshSceneEnlightenData>))]
public class MeshSceneEnlightenData
{
    [MetaMember("mSystemName")]
    public Symbol SystemName { get; set; }

    [MetaMember("mUVTransform")]
    public Vector4 UVTransform { get; set; }

    [MetaMember("mFlags")]
    public Flags  Flags { get; set; }
}

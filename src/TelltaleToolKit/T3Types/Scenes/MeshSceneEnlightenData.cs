using System.Numerics;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Scenes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<MeshSceneEnlightenData>))]
public class MeshSceneEnlightenData
{
    [MetaMember("mSystemName")]
    public Symbol SystemName { get; set; }

    [MetaMember("mUVTransform")]
    public Vector4 UVTransform { get; set; }

    [MetaMember("mFlags")]
    public Flags  Flags { get; set; }
}
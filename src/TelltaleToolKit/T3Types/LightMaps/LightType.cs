using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.LightMaps;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LightType>))]
public struct LightType
{
    [MetaMember("mLightType")] public int Type { get; set; }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.LightMaps;

[MetaSerializer(typeof(MetaClassSerializer<LightType>))]
public struct LightType
{
    [MetaMember("mLightType")]
    public int Type { get; set; }
}

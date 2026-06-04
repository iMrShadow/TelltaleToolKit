using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous.PreloadPackage;

[MetaSerializer(typeof(MetaClassSerializer<ResourceSeenTimes>))]
public class ResourceSeenTimes
{
    [MetaMember("mfEarliest")]
    public float Earliest { get; set; }

    [MetaMember("mfLatest")]
    public float Latest { get; set; }

    [MetaMember("mAdditionalScenes")]
    public HashSet<Symbol> AdditionalScenes { get; set; } = [];
}

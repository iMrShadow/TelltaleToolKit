using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Enlighten;

[MetaSerializer(typeof(MetaClassSerializer<EnlightenData>))]
public class EnlightenData
{
    [MetaMember("mSignature")]
    public EnlightenSignature Signature { get; set; } = new();

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mSystemData")]
    public List<EnlightenSystemData> SystemData { get; set; } = [];

    [MetaMember("mProbeData")]
    public List<EnlightenProbeData> ProbeData { get; set; } = [];
}

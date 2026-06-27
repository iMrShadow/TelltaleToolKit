using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Enlighten;

[MetaSerializer(typeof(MetaClassSerializer<EnlightenProbeData>))]
public class EnlightenProbeData
{
    [MetaMember("mEnvTileName")]
    public Symbol EnvTileName { get; set; } = Symbol.Empty;

    [MetaMember("mRadProbeSetCore")]
    public BinaryBuffer RadProbeSetCore { get; set; } = new();
    // Enlighten::RadProbeSetCore mpRadProbeSetCore;//ptr but not in lib. this is the format of the data below
}

// struct RadProbeSetCore {
//     RadProbeSetMetaData m_MetaData;
//     RadDataBlock m_ProbeSetPrecomp;
//     RadDataBlock m_EntireProbeSetPrecomp;
//     RadDataBlock m_InterpolationData;
//     RadDataBlock m_VisibilityData;
//     RadDataBlock m_DebugData;
// };

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

/// <summary>
///     Main class for .tmap files.
/// </summary>
[MetaSerializer(typeof(MetaClassSerializer<TransitionMap>))]
public class TransitionMap
{
    // Symbols can be both strings and crc64s.
    [MetaMember("mTransitionRemappers")]
    public Dictionary<Symbol, TransitionMapInfo> TransitionRemappersS { get; set; } = [];

    [MetaMember("mTransitionRemappers")]
    public Dictionary<string, TransitionMapInfo> TransitionRemappers { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<TransitionMapInfo>))]
    public class TransitionMapInfo
    {
        [MetaMember("mRemapper")]
        public TransitionRemapper Remapper { get; set; } = new();
    }
}

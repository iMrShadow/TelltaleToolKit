using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

/// <summary>
/// Main class for .tmap files.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TransitionMap>))]
public class TransitionMap
{
    // Symbols can be both strings and crc64s.
    [MetaMember("mRemapper")]
    public Dictionary<Symbol, TransitionMapInfo> TransitionRemappers = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TransitionMapInfo>))]
    public class TransitionMapInfo
    {
        [MetaMember("mRemapper")]
        public TransitionRemapper Remapper = new();
    }
}
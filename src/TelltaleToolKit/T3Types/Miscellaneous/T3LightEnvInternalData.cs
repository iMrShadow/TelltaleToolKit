using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<T3LightEnvInternalData>))]
public class T3LightEnvInternalData
{
    [MetaMember("mEntryForQuality[0]")]
    public QualityEntry EntryForQuality0 { get; set; } = new();

    [MetaMember("mEntryForQuality[1]")]
    public QualityEntry EntryForQuality1 { get; set; } = new();

    [MetaMember("mEntryForQuality[2]")]
    public QualityEntry EntryForQuality2 { get; set; } = new();

    [MetaMember("mEntryForQuality[3]")]
    public QualityEntry EntryForQuality3 { get; set; } = new();

    [MetaMember("mStationaryLightIndex")]
    public int StationaryLightIndex { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<QualityEntry>))]
    public class QualityEntry
    {
        [MetaMember("mShadowLayer")]
        public uint ShadowLayer { get; set; }

        [MetaMember("mFlags")]
        public Flags Flags { get; set; }
    }
}

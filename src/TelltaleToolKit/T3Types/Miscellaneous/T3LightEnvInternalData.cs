using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3LightEnvInternalData>))]
public class T3LightEnvInternalData
{
    [MetaMember("mEntryForQuality[0]")]
    public QualityEntry EntryForQuality0 { get; set; }

    [MetaMember("mEntryForQuality[1]")]
    public QualityEntry EntryForQuality1 { get; set; }

    [MetaMember("mEntryForQuality[2]")]
    public QualityEntry EntryForQuality2 { get; set; }

    [MetaMember("mEntryForQuality[3]")]
    public QualityEntry EntryForQuality3 { get; set; }

    [MetaMember("mStationaryLightIndex")]
    public int StationaryLightIndex { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<QualityEntry>))]
    public class QualityEntry
    {
        [MetaMember("mShadowLayer")]
        public uint ShadowLayer { get; set; }

        [MetaMember("mFlags")]
        public Flags Flags { get; set; }
    }
}
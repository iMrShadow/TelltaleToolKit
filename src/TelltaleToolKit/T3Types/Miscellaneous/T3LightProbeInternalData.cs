using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<T3LightProbeInternalData>))]
public class T3LightProbeInternalData
{
    [MetaMember("mEntryForQuality[0]")]
    public QualityEntry EntryForQuality0 { get; set; } = new();

    [MetaMember("mEntryForQuality[1]")]
    public QualityEntry EntryForQuality1 { get; set; } = new();

    [MetaMember("mEntryForQuality[2]")]
    public QualityEntry EntryForQuality2 { get; set; } = new();

    [MetaMember("mEntryForQuality[3]")]
    public QualityEntry EntryForQuality3 { get; set; } = new();

    [MetaSerializer(typeof(MetaClassSerializer<QualityEntry>))]
    public class QualityEntry
    {
        [MetaMember("mShadowTextureScale")]
        public Vector3 ShadowLayer { get; set; }

        [MetaMember("mShadowTextureBias")]
        public Vector3 Flags { get; set; }
    }
}

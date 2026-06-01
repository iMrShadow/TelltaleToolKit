using System.Numerics;
using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3LightProbeInternalData>))]
public class T3LightProbeInternalData
{
    [MetaMember("mEntryForQuality[0]")]
    public QualityEntry EntryForQuality0 { get; set; }

    [MetaMember("mEntryForQuality[1]")]
    public QualityEntry EntryForQuality1 { get; set; }

    [MetaMember("mEntryForQuality[2]")]
    public QualityEntry EntryForQuality2 { get; set; }

    [MetaMember("mEntryForQuality[3]")]
    public QualityEntry EntryForQuality3 { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<QualityEntry>))]
    public class QualityEntry
    {
        [MetaMember("mShadowTextureScale")]
        public Vector3 ShadowLayer { get; set; }

        [MetaMember("mShadowTextureBias")]
        public Vector3 Flags { get; set; }
    }
}

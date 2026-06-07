using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.LightMaps;

[MetaSerializer(typeof(MetaClassSerializer<T3LightSceneInternalData>))]
public class T3LightSceneInternalData
{
    [MetaMember("mEntryForQuality[0]")]
    public QualityEntry EntryForQuality0 { get; set; }

    [MetaMember("mEntryForQuality[1]")]
    public QualityEntry EntryForQuality1 { get; set; }

    [MetaMember("mEntryForQuality[2]")]
    public QualityEntry EntryForQuality2 { get; set; }

    [MetaMember("mEntryForQuality[3]")]
    public QualityEntry EntryForQuality3 { get; set; }

    [MetaMember("mStationaryLightCount")]
    public uint StationaryLightCount { get; set; }

    [MetaMember("mBakeVersion")]
    public uint BakeVersion { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<QualityEntry>))]
    public class QualityEntry
    {
        [MetaMember("mLightmapPages")]
        public List<LightmapPage> mLightmapPages { get; set; }

        [MetaMember("mhStaticShadowVolumeTexture")]
        public Handle<T3Texture> mhStaticShadowVolumeTexture { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<LightmapPage>))]
    public class LightmapPage
    {
        [MetaMember("mhTextureAtlas")]
        public Handle<T3Texture> TextureAtlas { get; set; }

        [MetaMember("mFlags")]
        public Flags Flags { get; set; }
    }
}

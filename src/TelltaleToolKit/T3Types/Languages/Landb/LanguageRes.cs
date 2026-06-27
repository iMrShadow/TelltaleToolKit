using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaSerializer(typeof(MetaClassSerializer<LanguageRes>))]
public class LanguageRes
{
    [MetaMember("mResName")]
    public Symbol ResName { get; set; }

    [MetaMember("mID")]
    public uint ID { get; set; }

    [MetaMember("mIDAlias")]
    public uint IDAlias { get; set; }

    [MetaMember("mhAnimation")]
    public Handle<Animation> Animation { get; set; }

    [MetaMember("mhVoiceData")]
    public Handle<SoundData> VoiceData { get; set; }

    [MetaMember("mLocalData")]
    public List<LanguageResLocal> LocalData { get; set; }

    [MetaMember("mLengthOverride")]
    public float LengthOverride { get; set; }

    [MetaMember("mResolvedLocalData")]
    public LanguageResLocal ResolvedLocalData { get; set; }

    [MetaMember("mRecordingStatus")]
    public EnumRecordingStatus RecordingStatus { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;
using TelltaleToolKit.T3Types.Voice;

namespace TelltaleToolKit.T3Types.Languages.Langdb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageResource>))]
public class LanguageResource
{
    [MetaMember("mId")]
    public int Id { get; set; }

    [MetaMember("mPrefix")]
    public string Prefix { get; set; } = string.Empty;

    [MetaMember("mText")]
    public string Text { get; set; } = string.Empty;

    [MetaMember("mhAnimation")]
    public Handle<Animation> Animation { get; set; } = new();

    [MetaMember("mhVoiceData")]
    public Handle<VoiceData> VoiceData { get; set; } = new();

    [MetaMember("mShared")]
    public bool Shared { get; set; }

    [MetaMember("mAllowSharing")]
    public bool AllowSharing { get; set; }

    [MetaMember("mbNoAnim")]
    public bool NoAnim { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();
    public Symbol ResName => new($"{Id}.langres");
}
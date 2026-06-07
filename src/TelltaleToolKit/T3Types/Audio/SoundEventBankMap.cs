using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

//.SOUNDEVENTBANKMAP FILES
[MetaSerializer(typeof(MetaClassSerializer<SoundEventBankMap>))]
public class SoundEventBankMap
{
    [MetaMember("mbLoadAllBanksGlobally")]
    public bool LoadAllBanksGlobally { get; set; }

    [MetaMember("mBankMap")]
    public Dictionary<string, List<string>> BankMap { get; set; } = [];
}

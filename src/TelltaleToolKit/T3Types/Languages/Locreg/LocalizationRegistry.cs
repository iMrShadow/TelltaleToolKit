using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Languages.Locreg;

// .locreg
[MetaSerializer(typeof(MetaClassSerializer<LocalizationRegistry>))]
public class LocalizationRegistry
{
    [MetaMember("mFlagIndexMap")]
    public Dictionary<Symbol, int> FlagIndexMap { get; set; } = [];

    [MetaMember("mFlagIndexMapReverse")]
    public Dictionary<int, Symbol> FlagIndexMapReverse { get; set; } = [];

    [MetaMember("mToolProps")]
    public ToolProps ToolProps { get; set; } = new();
}

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
    public Dictionary<Symbol, int> FlagIndexMapS { get; set; } = [];

    [MetaMember("mFlagIndexMapReverse")]
    public Dictionary<int, Symbol> FlagIndexMapReverseS { get; set; } = [];

    [MetaMember("mFlagIndexMap")]
    public Dictionary<string, int> FlagIndexMap { get; set; } = [];

    [MetaMember("mFlagIndexMapReverse")]
    public Dictionary<int, string> FlagIndexMapReverse { get; set; } = [];

    [MetaMember("mToolProps")]
    public ToolProps ToolProps { get; set; } = new();
}

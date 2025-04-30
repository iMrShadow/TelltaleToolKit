using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Rules;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LogicGroup>))]
public class LogicGroup
{
    [MetaMember("mOperator")]
    public int Operator { get; set; }

    [MetaMember("mItems")]
    public Dictionary<string, LogicItem> Items { get; set; } = [];

    [MetaMember("mLogicGroups")]
    public List<LogicGroup> LogicGroups { get; set; } = [];

    [MetaMember("mGroupOperator")]
    public int GroupOperator { get; set; }

    [MetaMember("mType")]
    public int Type { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LogicItem>))]
    public class LogicItem
    {
        [MetaMember("Baseclass_PropertySet")]
        public PropertySet BaseclassPropertySet { get; set; } = new();

        [MetaMember("mName")]
        public string Name { get; set; } = string.Empty;

        [MetaMember("mKeyNegateList")]
        public Dictionary<Symbol, bool> GroupOperator { get; set; } = []; // also a string/symbol 
    }
}
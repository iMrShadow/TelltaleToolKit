using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Rules;

[MetaSerializer(typeof(MetaClassSerializer<LogicGroup>))]
public class LogicGroup
{
    [MetaMember("mOperator")]
    public int Operator { get; set; }

    [MetaMember("mItems")]
    public Dictionary<string, LogicItem> Items { get; set; } = new();

    [MetaMember("mLogicGroups")]
    public List<LogicGroup> LogicGroups { get; set; } = [];

    [MetaMember("mGroupOperator")]
    public int GroupOperator { get; set; }

    [MetaMember("mType")]
    public int Type { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaSerializer(typeof(MetaClassSerializer<LogicItem>))]
    public class LogicItem
    {
        [MetaMember("Baseclass_PropertySet")]
        public PropertySet BaseclassPropertySet { get; set; } = new();

        [MetaMember("mName")]
        public string Name { get; set; } = string.Empty;

        [MetaMember("mKeyNegateList")]
        public Dictionary<Symbol, bool> GroupOperatorS { get; set; } = new();

        // also a string/symbol

        [MetaMember("mKeyNegateList")]
        public Dictionary<string, bool> GroupOperator { get; set; } = new();

        [MetaMember("mKeyComparisonList")]
        public Dictionary<Symbol, int> KeyComparisonList { get; set; } = new();

        [MetaMember("mKeyActionList")]
        public Dictionary<Symbol, int> KeyActionList { get; set; } = new();

        [MetaMember("mReferenceKeyList")]
        public List<string> ReferenceKeyList { get; set; } = [];
    }
}

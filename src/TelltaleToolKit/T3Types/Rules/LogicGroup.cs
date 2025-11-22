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
    public Dictionary<string, LogicItem> Items { get; set; } = new();

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
        public Dictionary<Symbol, bool> GroupOperator { get; set; } = new(); // also a string/symbol 
        
        [MetaMember("mKeyComparisonList")]
        public Dictionary<Symbol, int> KeyComparisonList { get; set; } = new(); // also a string/symbol 

        [MetaMember("mKeyActionList")]
        public Dictionary<Symbol, int> KeyActionList { get; set; } = new(); // also a string/symbol 

        [MetaMember("mReferenceKeyList")]
        public List<string> ReferenceKeyList { get; set; } = []; // also a string/symbol 
    }
}
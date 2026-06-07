using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Rules;

[MetaSerializer(typeof(Serializer))]
public class Rules
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();

    [MetaMember("mhLogicProps")]
    public Handle<PropertySet> LogicProps { get; set; } = new();

    [MetaMember("mRuleMap")]
    public Dictionary<string, Rule> RuleMap { get; set; } = [];

    public class Serializer : MetaSerializer<Rules>
    {
        private static readonly MetaClassSerializer<Rules> s_metaClassSerializer = new();

        public override void PreSerialize(ref Rules? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new Rules();

        public override void Serialize(ref Rules obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.RuleMap.Count);

                foreach (string ruleName in obj.RuleMap.Keys)
                {
                    stream.Write(ruleName);
                }

                foreach (Rule? ruleValue in obj.RuleMap.Values)
                {
                    Rule rule = ruleValue;
                    stream.Serialize(ref rule);
                }
            }
            else
            {
                obj.RuleMap.Clear();
                int numRules = stream.ReadInt32();

                List<string> keys = new(numRules);
                for (int i = 0; i < numRules; i++)
                {
                    string ruleName = stream.ReadString();
                    keys.Add(ruleName);
                }

                for (int i = 0; i < numRules; i++)
                {
                    Rule rule = new();
                    stream.Serialize(ref rule);
                    obj.RuleMap[keys[i]] = rule;
                }
            }

            stream.EndBlock();
        }
    }
}

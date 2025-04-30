using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Rules;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class Rules
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();

    [MetaMember("mhLogicProps")]
    public Handle<PropertySet> LogicProps { get; set; } = new();

    public HashSet<string> RulesSet { get; set; } = [];

    [MetaMember("mRuleMap")]
    public Dictionary<string, Rule> RuleMap { get; set; } = [];

    public class Serializer : MetaClassSerializer<Rules>
    {
        private static readonly DefaultClassSerializer<Rules> DefaultSerializer = new();

        public override void Serialize(ref Rules obj, MetaStream stream)
        {
            PreSerialize(ref obj, stream, null);
            DefaultSerializer.Serialize(ref obj, stream);
            stream.BeginBlock();

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                HashSet<string> rulesSet = obj.RulesSet;

                TTKContext.Instance().GetSerializer<HashSet<string>>()
                    .Serialize(ref rulesSet, stream);

                ////

                MetaClassSerializer<Rule> ruleSerializer = TTKContext.Instance().GetSerializer<Rule>();

                obj.RuleMap = new Dictionary<string, Rule>();

                foreach (string t in obj.RulesSet)
                {
                    var rule = new Rule();
                    ruleSerializer.Serialize(ref rule, stream);

                    obj.RuleMap.Add(t, rule);
                }
            }

            stream.EndBlock();
        }
    }
}
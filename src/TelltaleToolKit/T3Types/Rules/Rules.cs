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

    public HashSet<string> RulesSet { get; set; } = [];

    [MetaMember("mRuleMap")]
    public Dictionary<string, Rule> RuleMap { get; set; } = new();

    public class Serializer : MetaSerializer<Rules>
    {
        private static readonly MetaClassSerializer<Rules> s_metaClassSerializer = new();

        public override void Serialize(ref Rules obj, MetaStream stream)
        {
            PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);
            stream.BeginBlock();

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                HashSet<string> rulesSet = obj.RulesSet;

                stream.Serialize(ref rulesSet);

                ////

                obj.RuleMap = new Dictionary<string, Rule>();

                foreach (string t in obj.RulesSet)
                {
                    var rule = new Rule();
                    stream.Serialize(ref rule);

                    obj.RuleMap.Add(t, rule);
                }
            }

            stream.EndBlock();
        }
    }
}

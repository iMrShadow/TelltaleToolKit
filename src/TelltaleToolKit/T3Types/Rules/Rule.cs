using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Rules;

[MetaSerializer(typeof(Serializer))]
public class Rule
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mRuntimePropName")]
    public string RuntimePropName { get; set; } = string.Empty;

    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();

    [MetaMember("mConditionalProperties")]
    public PropertySet ConditionalProperties { get; set; } = new();

    [MetaMember("mActionProperties")]
    public PropertySet ActionProperties { get; set; } = new();

    [MetaMember("mbVersionHasAgents")]
    public bool VersionHasAgents { get; set; }

    [MetaMember("mConditions")]
    public LogicGroup Conditions { get; set; } = new();

    [MetaMember("mActions")]
    public LogicGroup Actions { get; set; } = new();

    [MetaMember("mElse")]
    public LogicGroup Else { get; set; } = new();

    [MetaMember("mAgentCategory")]
    public string AgentCategory { get; set; } = string.Empty;

    public List<AgentInfo> AgentInformation { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<AgentInfo>))]
    public class AgentInfo
    {
        [MetaMember("mAgentName")]
        public string AgentName { get; set; } = string.Empty;

        [MetaMember("mConditionalProperties")]
        public PropertySet ConditionalProperties { get; set; } = new();

        [MetaMember("mActionProperties")]
        public PropertySet ActionProperties { get; set; } = new();
    }

    public class Serializer : MetaSerializer<Rule>
    {
        private static readonly MetaClassSerializer<Rule> s_metaClassSerializer = new();

        public override void PreSerialize(ref Rule? obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new Rule();
            }
        }

        public override void Serialize(ref Rule obj, MetaStream stream)
        {
            PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (!obj.VersionHasAgents)
            {
                return;
            }

            stream.BeginBlock();
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                // var numAgents = stream.ReadInt32();

                List<AgentInfo> agents = obj.AgentInformation;
                Toolkit.Instance.GetSerializer<List<AgentInfo>>().Serialize(ref agents, stream);

                // for (int i = 0; i < numAgents; i++)
                // {
                //     var agent = new Rule.AgentInfo();
                //     MetaClass? ruleDesc = stream.GetMetaClass(typeof(Rule));
                //
                //
                // }
                //
                //
                //
                // obj.RuleMap = rulesDic;
            }

            stream.EndBlock();
        }
    }
}

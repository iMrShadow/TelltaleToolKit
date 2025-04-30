using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Rules;

[MetaClassSerializerGlobal(typeof(Serializer))]
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

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AgentInfo>))]
    public class AgentInfo
    {
        [MetaMember("mAgentName")]
        public string AgentName { get; set; } = string.Empty;

        [MetaMember("mConditionalProperties")]
        public PropertySet ConditionalProperties { get; set; } = new();

        [MetaMember("mActionProperties")]
        public PropertySet ActionProperties { get; set; } = new();
    }

    public class Serializer : MetaClassSerializer<Rule>
    {
        private static readonly DefaultClassSerializer<Rule> DefaultSerializer = new();

        public override void PreSerialize(ref Rule obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new Rule();
            }
        }

        public override void Serialize(ref Rule obj, MetaStream stream)
        {
            PreSerialize(ref obj, stream, null);
            DefaultSerializer.Serialize(ref obj, stream);

            if (!obj.VersionHasAgents)
            {
                return;
            }

            stream.BeginBlock();
            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // var numAgents = streamReader.ReadInt32();

                List<AgentInfo> agents = obj.AgentInformation;
                TTKContext.Instance().GetSerializer<List<AgentInfo>>().Serialize(ref agents, stream);

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
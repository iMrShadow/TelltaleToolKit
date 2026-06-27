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

        public override void PreSerialize(ref Rule? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new Rule();

        public override void Serialize(ref Rule obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (!obj.VersionHasAgents)
            {
                return;
            }

            if (!stream.GetMetaClass(typeof(Rule))!.ContainsMember("mbVersionHasAgents"))
                return;

            stream.BeginBlock();
            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.AgentInformation.Count);

                foreach (var agentInfo in obj.AgentInformation)
                {
                    AgentInfo info = agentInfo;
                    stream.Serialize(ref info);
                }
            }
            else
            {
                int numAgents = stream.ReadInt32();

                obj.AgentInformation = [];
                obj.AgentInformation.Capacity = numAgents;
                for (int i = 0; i < numAgents; i++)
                {
                    var agent = new AgentInfo();
                    stream.Serialize(ref agent);
                    obj.AgentInformation.Add(agent);
                }
            }

            stream.EndBlock();
        }
    }
}

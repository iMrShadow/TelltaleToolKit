using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Scenes;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class Scene
{
    [MetaMember("mbHidden")]
    public bool Hidden { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mReferencedScenes")]
    public List<Handle<Scene>> ReferencedScenes { get; set; } = [];

    public List<AgentInfo> Agents { get; set; } = [];

    public class Serializer : MetaClassSerializer<Scene>
    {
        private static readonly DefaultClassSerializer<Scene> DefaultSceneSerializer = new();

        public override void Serialize(ref Scene obj, MetaStream stream)
        {
            DefaultSceneSerializer.PreSerialize(ref obj, stream);
            DefaultSceneSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int numAgents = streamReader.ReadInt32();

                obj.Agents.Capacity = numAgents;
                for (var i = 0; i < numAgents; i++)
                {
                    var agentInfo = new AgentInfo();
                    TTKContext.Instance().GetSerializer<AgentInfo>().Serialize(ref agentInfo, stream);
                    obj.Agents.Add(agentInfo);
                }
            }

            stream.EndBlock();
        }
    }


    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AgentInfo>))]
    public class AgentInfo
    {
        [MetaMember("mAgentName")]
        public string AgentName { get; set; } = string.Empty;

        [MetaMember("mAgentSceneProps")]
        public PropertySet AgentSceneProps { get; set; } = new();

        // Below the members are for the original games.
        // In newer games, hey have been moved to the properties.

        [MetaMember("mStartPos")]
        public Vector3 StartPos { get; set; } = new();

        [MetaMember("mStartQuat")]
        public Quaternion StartQuat { get; set; } = new();

        [MetaMember("mbVisible")]
        public bool Visible { get; set; }

        [MetaMember("mbTransient")]
        public bool Transient { get; set; }

        [MetaMember("mbAttached")]
        public bool Attached { get; set; }

        [MetaMember("mAttachAgentNode")]
        public string AttachAgentNode { get; set; } = string.Empty;

        [MetaMember("mAttachAgent")]
        public string AttachAgent { get; set; } = string.Empty;

        [MetaMember("mbMembersImportedIntoSceneProps")]
        public bool MembersImportedIntoSceneProps { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AgentQualitySettings>))]
    public class AgentQualitySettings
    {
        [MetaMember("mFlags")]
        public Flags Flags { get; set; }
    }
}
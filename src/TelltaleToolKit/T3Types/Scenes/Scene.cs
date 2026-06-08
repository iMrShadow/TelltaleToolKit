using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Scenes;

[MetaSerializer(typeof(Serializer))]
public class Scene
{
    [MetaMember("mbHidden")]
    public bool Hidden { get; set; }

    [MetaMember("mbAfterEffectEnabled")]
    public bool AfterEffectEnabled { get; set; }

    [MetaMember("mb2DShadowEnabled")]
    public bool _2DShadowEnabled { get; set; }

    [MetaMember("m2DShadowSize")]
    public float _2DShadowSize { get; set; }

    [MetaMember("m2DShadowCIntensity")]
    public float m2DShadowCIntensity { get; set; }

    [MetaMember("m2DShadowBIntensity")]
    public float m2DShadowBIntensity { get; set; }

    [MetaMember("m2DShadowZMin")]
    public float m2DShadowZMin { get; set; }

    [MetaMember("m2DShadowZMax")]
    public float m2DShadowZMax { get; set; }

    [MetaMember("mFXColorActive")]
    public bool FXColorActive { get; set; }

    [MetaMember("mFXColor")]
    public Color FXColor { get; set; } = new();

    [MetaMember("mFXColorOpacity")]
    public float FXColorOpacity { get; set; }

    [MetaMember("mFXLevelsActive")]
    public bool mFXLevelsActive { get; set; }

    [MetaMember("mFXLevelsBlack")]
    public float FXLevelsBlack { get; set; }

    [MetaMember("mFXLevelsWhite")]
    public float FXLevelsWhite { get; set; }

    [MetaMember("mFXLevelsIntensity")]
    public float FXLevelsIntensity { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mGlowClearColor")]
    public Color GlowClearColor { get; set; } = new();

    [MetaMember("mReferencedScenes")]
    public List<Handle<Scene>> ReferencedScenes { get; set; } = [];

    public List<AgentInfo> Agents { get; set; } = [];

    public class Serializer : MetaSerializer<Scene>
    {
        private static readonly MetaClassSerializer<Scene> s_metaClassSceneSerializer = new();

        public override void Serialize(ref Scene obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSceneSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSceneSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Agents.Count);

                foreach (AgentInfo? agent in obj.Agents)
                {
                    AgentInfo agentInfo = agent;
                    stream.Serialize(ref agentInfo);
                }
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int numAgents = stream.ReadInt32();

                obj.Agents.Capacity = numAgents;
                for (int i = 0; i < numAgents; i++)
                {
                    AgentInfo agentInfo = new();
                    stream.Serialize(ref agentInfo);
                    obj.Agents.Add(agentInfo);
                }
            }

            stream.EndBlock();
        }
    }

    [MetaSerializer(typeof(MetaClassSerializer<AgentInfo>))]
    public class AgentInfo
    {
        [MetaMember("mAgentName")]
        public string AgentName { get; set; } = string.Empty;

        [MetaMember("mAgentSceneProps")]
        public PropertySet AgentSceneProps { get; set; } = new();

        // Below the members are for the original games.
        // In newer games, they have been moved to the properties.

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

    [MetaSerializer(typeof(MetaClassSerializer<AgentQualitySettings>))]
    public class AgentQualitySettings
    {
        [MetaMember("mFlags")]
        public Flags Flags { get; set; }
    }

    internal class AddSceneInfo
    {
    }
}

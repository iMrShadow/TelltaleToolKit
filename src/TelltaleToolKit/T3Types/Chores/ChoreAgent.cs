using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Chores;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ChoreAgent>))]
public class ChoreAgent
{
    [MetaMember("mAgentName")]
    public string AgentName { get; set; } = string.Empty;

    [MetaMember("mResources")]
    public List<int> Resources { get; set; } = [];

    [MetaMember("mChoreProps")]
    public PropertySet ChoreProps { get; set; } = new();

    [MetaMember("mChorePosition")]
    public Vector3 ChorePosition { get; set; }

    [MetaMember("mChoreQuat")]
    public Quaternion ChoreQuat { get; set; } = new();

    [MetaMember("mFlags")]
    public Flags Flags { get; set; } // ??????????/

    [MetaMember("mAttachment")]
    public Attachment AttachmentWhat { get; set; } = new(); // ???????/

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Attachment>))]
    public class Attachment
    {
        [MetaMember("mbDoAttach")]
        public bool DoAttach { get; set; }

        [MetaMember("mAttachTo")]
        public string AttachTo { get; set; } = string.Empty;

        [MetaMember("mAttachToNode")]
        public string AttachToNode { get; set; } = string.Empty;

        [MetaMember("mAttachPos")]
        public Vector3 AttachPos { get; set; }

        [MetaMember("mAttachQuat")]
        public Quaternion AttachQuat { get; set; } = new();

        [MetaMember("mbAttachPreserveWorldPos")]
        public bool AttachPreserveWorldPos { get; set; }

        [MetaMember("mbLeaveAttachedWhenComplete")]
        public bool LeaveAttachedWhenComplete { get; set; }
    }
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class SaveGame
{
    [MetaMember("mLuaDoFile")]
    public string LuaDoFile { get; set; } = string.Empty;

    [MetaMember("mAgentInfo")]
    public List<AgentInfo> AgentInformation { get; set; } = new();

    [MetaMember("mRuntimePropNames")]
    public HashSet<string> RuntimePropNames { get; set; } = new();

    [MetaMember("mAdditionalPropNames")]
    public HashSet<string> AdditionalPropNames { get; set; } = new();

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AgentInfo>))]
    public class AgentInfo
    {
        [MetaMember("mAgentName")]
        public string AgentName { get; set; } = string.Empty;

        [MetaMember("mPosition")]
        public Vector3 Position { get; set; } = new();

        [MetaMember("mQuaternion")]
        public Quaternion Quaternion { get; set; } = new();

        [MetaMember("mbAttached")]
        public bool Attached { get; set; }

        [MetaMember("mAttachedToAgent")]
        public string AttachedToAgent { get; set; } = string.Empty;

        [MetaMember("mAttachedToNode")]
        public string AttachedToNode { get; set; } = string.Empty;
    }
    
    
    public class Serializer : MetaClassSerializer<SaveGame>
    {
        private static readonly DefaultClassSerializer<SaveGame> DefaultSaveGameSerializer = new();

        public override void Serialize(ref SaveGame obj, MetaStream stream)
        {
            DefaultSaveGameSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if (stream.GetMetaClass(typeof(PropertySet)) != null)
                {
                    // TODO: There are 3 property sets one after another in old games.
                    var propertySet = new PropertySet();
                    TTK.Serialize(ref propertySet, stream);
                }
            }
        }
    }
}


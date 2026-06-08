using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Events;

[MetaSerializer(typeof(Serializer))]
public class SaveGame
{
    [MetaMember("mLuaDoFile")]
    public string LuaDoFile { get; set; } = string.Empty;

    [MetaMember("mAgentInfo")]
    public List<AgentInfo> AgentInformation { get; set; } = [];

    [MetaMember("mRuntimePropNames")]
    public HashSet<string> RuntimePropNames { get; set; } = [];

    [MetaMember("mAdditionalPropNames")]
    public HashSet<string> AdditionalPropNames { get; set; } = [];

    [MetaMember("mRuntimePropNames")]
    public HashSet<Symbol> RuntimePropNamesS { get; set; } = [];

    [MetaMember("mEnabledDynamicSets")]
    public HashSet<Symbol> EnabledDynamicSets { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<AgentInfo>))]
    public class AgentInfo
    {
        [MetaMember("mAgentName")]
        public string AgentName { get; set; } = string.Empty;

        [MetaMember("mAgentName")]
        public Symbol AgentNameS { get; set; } = Symbol.Empty;

        [MetaMember("mSceneName")]
        public string  SceneName { get; set; } = string.Empty;

        [MetaMember("mSceneName")]
        public Symbol  SceneNameS { get; set; } = Symbol.Empty;

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

        [MetaMember("mAttachedToAgent")]
        public Symbol AttachedToAgentS { get; set; } = Symbol.Empty;

        [MetaMember("mAttachedToNode")]
        public Symbol AttachedToNodeS { get; set; } = Symbol.Empty;
    }


    public class Serializer : MetaSerializer<SaveGame>
    {
        private static readonly MetaClassSerializer<SaveGame> s_metaClassSaveGameSerializer = new();

        public override void Serialize(ref SaveGame obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSaveGameSerializer.PreSerialize(ref obj, stream);
            s_metaClassSaveGameSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                if (stream.GetMetaClass(typeof(PropertySet)) != null)
                {
                    // TODO: There are 3 property sets one after another in old games.
                    var propertySet = new PropertySet();
                    stream.Serialize(ref propertySet);
                }
            }
        }
    }
}

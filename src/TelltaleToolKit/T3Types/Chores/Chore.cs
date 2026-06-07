using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Scenes;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(Serializer))]
public class Chore
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mhChoreScene")]
    public Handle<Scene> ChoreScene { get; set; } = new();

    [MetaMember("mLength")]
    public float Length { get; set; }

    [MetaMember("mNumResources")]
    public int NumResources { get; set; }

    [MetaMember("mNumAgents")]
    public int NumAgents { get; set; }

    [MetaMember("mResourcesChore")]
    public List<ChoreResource> ResourcesChore { get; set; } = [];

    [MetaMember("mAgents")]
    public List<ChoreAgent> Agents { get; set; } = [];

    [MetaMember("mEditorProps")]
    public PropertySet EditorProps { get; set; } = new();

    [MetaMember("mbResetNavCamsOnExit")]
    public bool ResetNavCamsOnExit { get; set; }

    [MetaMember("mbChoreBackgroundFade")]
    public bool ChoreBackgroundFade { get; set; }

    [MetaMember("mbChoreBackgroundLoop")]
    public bool ChoreBackgroundLoop { get; set; }

    [MetaMember("mChoreSceneFile")]
    public string ChoreSceneFile { get; set; } = string.Empty;

    [MetaMember("mbEndPause")]
    public bool EndPause { get; set; }

    [MetaMember("mRenderDelay")]
    public int RenderDelay { get; set; }

    [MetaMember("mResources")]
    public List<ChoreResource> Resources { get; set; } = [];

    [MetaMember("mSynchronizedToLocalization")]
    public LocalizeInfo SynchronizedToLocalization { get; set; } = new();

    [MetaMember("mSynchronizedToLocalization")]
    public Symbol SynchronizedToLocalizationS { get; set; } = Symbol.Empty;

    [MetaMember("mDependencies")]
    public DependencyLoader Dependencies { get; set; } = new();

    [MetaMember("mToolProps")]
    public ToolProps ToolProps { get; set; } = new();

    [MetaMember("mWalkPaths")]
    public Dictionary<Symbol, WalkPath> WalkPaths { get; set; } = [];

    public class Serializer : MetaSerializer<Chore>
    {
        private static readonly MetaClassSerializer<Chore> s_metaClassSerializer = new();

        public override void Serialize(ref Chore obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                obj.NumResources = obj.ResourcesChore.Count;
                obj.NumAgents = obj.Agents.Count;
            }

            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            // In the oldest games, they have a DCArray for each type in their metaclass descriptions.
            // However, the serializers still read and write the resources.
            // Which means...they are either overwritten, duplicates or being totally useless.
            if (stream.Mode is MetaStreamMode.Write)
            {
                foreach (var res in obj.ResourcesChore)
                {
                    ChoreResource choreResource = res;
                    stream.Serialize(ref choreResource);
                }

                foreach (var agent in obj.Agents)
                {
                    ChoreAgent agent1 = agent;
                    stream.Serialize(ref agent1);
                }
            }
            else
            {
                obj.ResourcesChore.Clear();
                for (int i = 0; i < obj.NumResources; i++)
                {
                    var choreResource = new ChoreResource();
                    stream.Serialize(ref choreResource);
                    obj.ResourcesChore.Add(choreResource);
                }

                obj.Agents.Clear();
                for (int i = 0; i < obj.NumAgents; i++)
                {
                    var choreAgent = new ChoreAgent();
                    stream.Serialize(ref choreAgent);
                    obj.Agents.Add(choreAgent);
                }
            }
        }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumExtentsMode>))]
    public struct EnumExtentsMode
    {
        [MetaMember("mVal")]
        public EnumExtents Val { get; set; }
    }

    [MetaSerializer(typeof(EnumSerializer<EnumExtents>))]
    public enum EnumExtents
    {
        LangRes = 1,
        Spillout = 2
    }
}

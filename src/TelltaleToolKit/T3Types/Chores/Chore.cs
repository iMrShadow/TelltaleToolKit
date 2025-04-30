using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Scenes;

namespace TelltaleToolKit.T3Types.Chores;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class Chore
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

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
    public List<ChoreResource> Resources { get; set; }


    public class Serializer : MetaClassSerializer<Chore>
    {
        private static readonly DefaultClassSerializer<Chore> DefaultSerializer = new();


        public override void Serialize(ref Chore obj, MetaStream stream)
        {
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if (obj.NumResources > 0)
                {
                    // TODO: Check for Texas and Bone. They have DCArrays for Chore Resources

                    for (int i = 0; i < obj.NumResources; i++)
                    {
                        var choreResource = new ChoreResource();
                        TTKContext.Instance().GetSerializer<ChoreResource>().Serialize(ref choreResource, stream);
                        obj.ResourcesChore.Add(choreResource);
                    }
                }

                if (obj.NumAgents > 0)
                {
                    // TODO: Check for Texas and Bone. They have DCArrays for Chore Agents

                    for (var i = 0; i < obj.NumAgents; i++)
                    {
                        var choreAgent = new ChoreAgent();
                        TTKContext.Instance().GetSerializer<ChoreAgent>().Serialize(ref choreAgent, stream);
                        obj.Agents.Add(choreAgent);
                    }
                }
            }
        }
    }

    public class EnumExtentsMode
    {
    }
}
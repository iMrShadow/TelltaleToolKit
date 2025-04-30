using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Chores;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class ChoreResource
{
    [MetaMember("mResName")]
    public string ResName { get; set; } = string.Empty;

    [MetaMember("mResLength")]
    public float ResLength { get; set; }

    [MetaMember("mPriority")]
    public int Priority { get; set; }

    [MetaMember("mResourceGroup")]
    public string ResourceGroup { get; set; } = string.Empty;

    [MetaMember("mControlAnimation")]
    public Animation ControlAnimation { get; set; } = new();

    [MetaMember("mBlocks")]
    public List<Block> Blocks { get; set; } = [];

    [MetaMember("mbNoPose")]
    public bool NoPose { get; set; }

    [MetaMember("mbEmbedded")]
    public bool HasEmbedded { get; set; }

    [MetaMember("mbEnabled")]
    public bool Enabled { get; set; }

    [MetaMember("mbIsAgentResource")]
    public bool IsAgentResource { get; set; }

    [MetaMember("mbViewGraphs")]
    public bool ViewGraphs { get; set; }

    [MetaMember("mbViewProperties")]
    public bool ViewProperties { get; set; }

    [MetaMember("mbViewResourceGroups")]
    public bool ViewResourceGroups { get; set; }

    [MetaMember("mResourceProperties")]
    public PropertySet ResourceProperties { get; set; } = new();

    [MetaMember("mResourceGroupInclude")]
    public Dictionary<string, float> ResourceGroupInclude { get; set; } = [];

    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new(); // Bone only

    [MetaMember("mAAStatus")]
    public AutoActStatus AaStatus { get; set; } // Bone only

    [MetaMember("mhObject")]
    public HandleBase ObjectHandle { get; set; } = new(); // Bone only

    public object? Embedded { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Block>))]
    public class Block
    {
        [MetaMember("mStartTime")]
        public float StartTime { get; set; }

        [MetaMember("mEndTime")]
        public float EndTime { get; set; }

        [MetaMember("mbLoopingBlock")]
        public bool LoopingBlock { get; set; }

        [MetaMember("mScale")]
        public float Scale { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<AutoActStatus>))]
    public enum AutoActStatus
    {
    }

    public class Serializer : MetaClassSerializer<ChoreResource>
    {
        private static readonly DefaultClassSerializer<ChoreResource> DefaultSerializer = new();

        public override void Serialize(ref ChoreResource obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            MetaClass? description = stream.GetMetaClass(typeof(ChoreResource));

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if (description != null && description.ContainsMember("mbEmbedded") && obj.HasEmbedded)
                {
                    MetaClassType embeddedClassType = streamReader.ReadMetaClassType();
                    Symbol _ = streamReader.ReadSymbol();

                    object? embedded = Activator.CreateInstance(embeddedClassType.LinkingType);

                    TTKContext.Instance().GetSerializer(embeddedClassType.LinkingType).Serialize(ref embedded, stream);
                    obj.Embedded = embedded;
                }
            }
        }
    }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Rules;
using TelltaleToolKit.T3Types.StyleGuides;

namespace TelltaleToolKit.T3Types.Dialogs.Dlog;

[MetaSerializer(typeof(Serializer))]
public class DialogBase
{
    [MetaMember("mDialogElemType")]
    public DialogUtils.DialogElemT DialogElemType { get; set; }

    [MetaMember("mUID")]
    public int Uid { get; set; }

    [MetaMember("mUID")]
    public uint UnsignedUid { get; set; }

    [MetaMember("mVisConditions")]
    public PropertySet VisConditions { get; set; } = new();

    [MetaMember("mPostActions")]
    public PropertySet PostActions { get; set; } = new();

    [MetaMember("mhBackgroundChore")]
    public Handle<Chore> BackgroundChore { get; set; } = new();

    [MetaMember("mRule")]
    public Rule Rule { get; set; } = new();

    [MetaMember("mbHasStyleGuides")]
    public bool HasStyleGuides { get; set; }

    [MetaMember("mDependentVisBranch")]
    public string DependentVisBranch { get; set; } = string.Empty;

    [MetaMember("mDescriptiveName")]
    public string DescriptiveName { get; set; }

    [MetaMember("mFlags")]
    public Flags mFlags { get; set; }

    [MetaMember("mTaskID")]
    public uint mTaskID { get; set; }


    public int ActualId;
    public List<StyleGuideRef> StyleGuideRefs = [];

    public class Serializer : MetaSerializer<DialogBase>
    {

        private static readonly MetaClassSerializer<DialogBase> s_metaClassSerializer = new();

        public override void Serialize(ref DialogBase obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj.HasStyleGuides)
                {
                    Toolkit.Instance.GetSerializer<List<StyleGuideRef>>().Serialize(ref obj.StyleGuideRefs, stream);
                }
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                if (obj.HasStyleGuides)
                {
                    Toolkit.Instance.GetSerializer<List<StyleGuideRef>>().Serialize(ref obj.StyleGuideRefs, stream);
                }
            }
        }
    }
}

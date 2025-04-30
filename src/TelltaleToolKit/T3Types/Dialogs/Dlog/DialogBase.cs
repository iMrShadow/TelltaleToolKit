using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Rules;
using TelltaleToolKit.T3Types.StyleGuides;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(Serializer))]
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
    public string DependentVisBranch { get; set; } = String.Empty;

    public int ActualId;
    public List<StyleGuideRef> StyleGuideRefs = [];

    public class Serializer : MetaClassSerializer<DialogBase>
    {
        public override void Serialize(ref DialogBase obj, MetaStream stream)
        {
            new DefaultClassSerializer<DialogBase>().Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                if (obj.HasStyleGuides)
                {
                    TTKContext.Instance().GetSerializer<List<StyleGuideRef>>().Serialize(ref obj.StyleGuideRefs, stream);
                }
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if (obj.HasStyleGuides)
                {
                    TTKContext.Instance().GetSerializer<List<StyleGuideRef>>().Serialize(ref obj.StyleGuideRefs, stream);
                }
            }
        }
    }
}
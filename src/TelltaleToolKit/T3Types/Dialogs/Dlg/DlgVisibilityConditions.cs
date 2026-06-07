using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Rules;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class DlgVisibilityConditions
{
    public Rule Rule { get; set; }

    [MetaMember("mbDiesOff")]
    public bool DiesOff { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mDownstreamVisCond")]
    public DlgDownstreamVisibilityConditions DownstreamVisCond { get; set; }

    [MetaMember("mScriptVisCond")]
    public string ScriptVisCond { get; set; }

    public class Serializer : MetaSerializer<DlgVisibilityConditions>
    {
        private static readonly MetaClassSerializer<DlgVisibilityConditions> s_metaClassSerializer = new();

        public override void Serialize(ref DlgVisibilityConditions obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                if ((obj.Flags.Data & 1) == 0)
                    return;

                Rule objRule = obj.Rule;
                stream.Serialize(ref objRule);
            }
        }
    }
}

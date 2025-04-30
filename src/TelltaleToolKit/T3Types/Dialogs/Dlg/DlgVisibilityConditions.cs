using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Rules;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
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

    public class Serializer : MetaClassSerializer<DlgVisibilityConditions>
    {
        private static readonly DefaultClassSerializer<DlgVisibilityConditions> DefaultSerializer = new();

        public override void Serialize(ref DlgVisibilityConditions obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if ((obj.Flags.Data & 1) == 0)
                    return;

                Rule objRule = obj.Rule;
                TTK.PreSerialize(ref objRule, stream);
                TTK.Serialize(ref objRule, stream);
            }
        }
    }
}
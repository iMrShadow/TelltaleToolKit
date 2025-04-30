using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgDownstreamVisibilityConditions>))]
public class DlgDownstreamVisibilityConditions
{
    [MetaMember("mNodeTypeFlags")]
    public Flags NodeTypeFlags { get; set; }

    [MetaMember("mMaxNumNodeEvals")]
    public int MaxNumNodeEvals { get; set; }
}
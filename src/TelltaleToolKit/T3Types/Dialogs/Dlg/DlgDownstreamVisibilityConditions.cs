using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgDownstreamVisibilityConditions>))]
public class DlgDownstreamVisibilityConditions
{
    [MetaMember("mNodeTypeFlags")]
    public Flags NodeTypeFlags { get; set; }

    [MetaMember("mMaxNumNodeEvals")]
    public int MaxNumNodeEvals { get; set; }
}

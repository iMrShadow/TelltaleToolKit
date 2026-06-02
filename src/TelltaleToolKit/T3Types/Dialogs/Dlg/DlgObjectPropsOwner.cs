using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgObjectPropsOwner>))]
public class DlgObjectPropsOwner
{
    [MetaMember("mDlgObjectProps")]
    public DlgObjectProps DlgObjectProps { get; set; }
}

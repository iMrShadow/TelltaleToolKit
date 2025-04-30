using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgObjectPropsOwner>))]
public class DlgObjectPropsOwner
{
    [MetaMember("mDlgObjectProps")]
    public DlgObjectProps DlgObjectProps { get; set; }
}
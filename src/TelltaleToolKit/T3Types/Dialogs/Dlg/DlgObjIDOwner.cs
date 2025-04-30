using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgObjIDOwner>))]
public class DlgObjIDOwner
{
    [MetaMember("mDlgObjID")]
    public DlgObjId DlgObjID { get; set; }
}
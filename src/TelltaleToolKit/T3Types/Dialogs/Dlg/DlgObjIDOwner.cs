using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgObjIDOwner>))]
public class DlgObjIDOwner
{
    [MetaMember("mDlgObjID")]
    public DlgObjId DlgObjID { get; set; } = new();
}

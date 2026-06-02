using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgChoicesChildPre>))]
public class DlgChoicesChildPre : IDlgChild
{
    [MetaMember("Baseclass_DlgChild")]
    public DlgChild DlgChild { get; set; }
}

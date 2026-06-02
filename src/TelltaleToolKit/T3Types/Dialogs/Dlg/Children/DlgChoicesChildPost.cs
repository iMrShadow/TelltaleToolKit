using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgChoicesChildPost>))]
public class DlgChoicesChildPost : IDlgChild
{
    [MetaMember("Baseclass_DlgChild")]
    public DlgChild DlgChild { get; set; }
}

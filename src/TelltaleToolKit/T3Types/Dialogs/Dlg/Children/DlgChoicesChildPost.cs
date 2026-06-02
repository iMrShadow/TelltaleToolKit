using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgChoicesChildPost>))]
public class DlgChoicesChildPost : IDlgChild
{
    [MetaMember("Baseclass_DlgChild")]
    public DlgChild DlgChild { get; set; }
}

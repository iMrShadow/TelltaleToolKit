using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgObjId>))]
public class DlgObjId
{
    [MetaMember("mID")]
    public Symbol Id { get; set; }
}

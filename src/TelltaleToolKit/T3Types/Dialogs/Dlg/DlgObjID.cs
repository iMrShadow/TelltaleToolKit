using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgObjId>))]
public class DlgObjId
{
    [MetaMember("mID")]
    public Symbol Id { get; set; } = Symbol.Empty;
}

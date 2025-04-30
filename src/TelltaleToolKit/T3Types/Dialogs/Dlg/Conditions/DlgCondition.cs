using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgCondition>))]
public class DlgCondition : IDlgObjIdOwner
{
    [MetaMember("Baseclass_DlgObjIdOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }
}
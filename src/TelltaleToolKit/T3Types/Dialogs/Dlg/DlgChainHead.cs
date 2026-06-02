using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgChainHead>))]
public class DlgChainHead : IDlgObjIdOwner
{
    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }

    [MetaMember("mLink")]
    public DlgNodeLink Link { get; set; }
}

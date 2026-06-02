using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeLink>))]
public class DlgNodeLink : IDlgObjIdOwner
{
    [MetaMember("mRequiredCCType")]
    public ChainContextTypeID RequiredCCType { get; set; }

    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<ChainContextTypeID>))]
public enum ChainContextTypeID
{
    // eCC
    Unspecified = 1,
    Action = 2,
    Data = 3
}

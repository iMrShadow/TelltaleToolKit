using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeLink>))]
public class DlgNodeLink : IDlgObjIdOwner
{
    [MetaMember("mRequiredCCType")]
    public ChainContextTypeID RequiredCCType { get; set; }

    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<ChainContextTypeID>))]
public enum ChainContextTypeID
{
    // eCC
    Unspecified = 1,
    Action = 2,
    Data = 3
}

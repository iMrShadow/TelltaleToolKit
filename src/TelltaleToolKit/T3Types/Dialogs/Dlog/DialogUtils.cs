using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

public class DialogUtils
{
    [MetaClassSerializerGlobal(typeof(EnumSerializer<DialogElemT>))]
    public enum DialogElemT : uint
    {
        Exchange = 1,
        Item = 2,
        Branch = 3,
        Dialog = 4,
        Text = 5,
        Line = 0xFEEDFACE,
    }
}
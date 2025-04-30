using TelltaleToolKit.Reflection;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

public class DlgChildSetChoicesChildPre: IDlgChildSet
{
    [MetaMember("Baseclass_DlgChildSet")]
    public DlgChildSet DlgChildSet { get; set; }
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgFolder>))]
public class DlgFolder : IDlgObjIdOwner, IDlgObjectPropsOwner, IDlgChildSet, ITaskOwner
{
    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }

    [MetaMember("Baseclass_DlgObjectPropsOwner")]
    public DlgObjectPropsOwner DlgObjectPropsOwner { get; set; }
    
    [MetaMember("Baseclass_DlgChildSet")]
    public DlgChildSet DlgChildSet { get; set; }
    
    [MetaMember("Baseclass_TaskOwner")]
    public TaskOwner TaskOwner { get; set; }

    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mProdReportProps")]
    public PropertySet ProdReportProps { get; set; }


}
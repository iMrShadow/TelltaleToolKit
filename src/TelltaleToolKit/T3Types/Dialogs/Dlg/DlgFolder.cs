using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgFolder>))]
public class DlgFolder : IDlgObjIdOwner, IDlgObjectPropsOwner, IDlgChildSet, ITaskOwner
{
    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; } = new();

    [MetaMember("Baseclass_DlgObjectPropsOwner")]
    public DlgObjectPropsOwner DlgObjectPropsOwner { get; set; }= new();

    [MetaMember("Baseclass_DlgChildSet")]
    public DlgChildSet DlgChildSet { get; set; }= new();

    [MetaMember("Baseclass_TaskOwner")]
    public TaskOwner TaskOwner { get; set; }= new();

    [MetaMember("mName")]
    public Symbol Name { get; set; } = Symbol.Empty;

    [MetaMember("mProdReportProps")]
    public PropertySet ProdReportProps { get; set; }= new();
}

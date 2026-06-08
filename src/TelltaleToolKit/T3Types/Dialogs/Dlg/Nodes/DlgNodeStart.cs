using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeStart>))]
public class DlgNodeStart : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaMember("mProdReportProps")]
    public PropertySet ProdReportProps { get; set; }

    [MetaMember("m_outputIndex")]
    public int OutputIndex { get; set; }
}

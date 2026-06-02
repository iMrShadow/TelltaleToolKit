using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgChildSetChoicesChildPost>))]
public class DlgChildSetChoicesChildPost : IDlgChildSet
{
    [MetaMember("Baseclass_DlgChildSet")]
    public DlgChildSet DlgChildSet { get; set; }
}

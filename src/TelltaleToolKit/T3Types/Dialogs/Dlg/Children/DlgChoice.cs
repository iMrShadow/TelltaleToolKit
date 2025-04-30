using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgChoice>))]
public class DlgChoice : IDlgChild, IDlgConditionSet
{
    [MetaMember("Baseclass_DlgChild")]

    public DlgChild DlgChild { get; set; }

    [MetaMember("Baseclass_DlgConditionSet")]

    public DlgConditionSet DlgConditionSet { get; set; }
}
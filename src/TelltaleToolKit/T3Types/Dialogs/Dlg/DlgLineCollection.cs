using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgLineCollection>))]
public class DlgLineCollection : IGenerator
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; }

    [MetaMember("mLines")]
    public Dictionary<int, DlgLine> Lines { get; set; } = new();
}

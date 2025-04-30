using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeChoices>))]
public class DlgNodeChoices : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }
    [MetaMember("mChoices")]
    public DlgChildSetChoice Choices { get; set; }

    [MetaMember("mPreChoice")]
    public DlgChildSetChoicesChildPre PreChoice { get; set; }

    [MetaMember("mPostChoice")]
    public DlgChildSetChoicesChildPost PostChoice { get; set; }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeParallel>))]
public class DlgNodeParallel : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaMember("mPElements")]
    public DlgChildSetElement PElements { get; set; }

    [MetaMember("mElemUseCriteria")]
    public DlgNodeCriteria ElemUseCriteria { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<PElement>))]
    public class PElement : IDlgChild
    {
        [MetaMember("Baseclass_DlgChild")]
        public DlgChild DlgChild { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<DlgChildSetElement>))]
    public class DlgChildSetElement : IDlgChildSet
    {
        [MetaMember("Baseclass_DlgChildSet")]
        public DlgChildSet DlgChildSet { get; set; }
    }
}

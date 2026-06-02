using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeParallel>))]
public class DlgNodeParallel : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaMember("mPElements")]
    public DlgChildSetElement PElements { get; set; }

    [MetaMember("mElemUseCriteria")]
    public DlgNodeCriteria ElemUseCriteria { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PElement>))]
    public class PElement : IDlgChild
    {
        [MetaMember("Baseclass_DlgChild")]
        public DlgChild DlgChild { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgChildSetElement>))]
    public class DlgChildSetElement : IDlgChildSet
    {
        [MetaMember("Baseclass_DlgChildSet")]
        public DlgChildSet DlgChildSet { get; set; }
    }
}

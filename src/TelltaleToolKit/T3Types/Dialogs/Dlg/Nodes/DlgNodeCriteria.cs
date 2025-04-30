using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeCriteria>))]
public class DlgNodeCriteria : IDlgNode
{
    [MetaClassSerializerGlobal(typeof(EnumSerializer<DefaultResult>))]
    public enum DefaultResult
    {
        DefaultToPass = 1,
        DefaultToNotPass = 2,
        DefaultToNotPassUnlessTransparent = 3
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<Test>))]
    public enum Test
    {
        Required = 1,
        Forbidden = 2
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<Threshold>))]
    public enum Threshold
    {
        Any = 1,
        All = 2
    }

    [MetaMember("mTestType")]
    public EnumTestT TestType { get; set; }


    [MetaMember("mFlagsThreshold")]
    public EnumThresholdT FlagsThreshold { get; set; }


    [MetaMember("mCriteriaThreshold")]
    public EnumThresholdT CriteriaThreshold { get; set; }

    [MetaMember("mDefaultResult")]
    public EnumDefaultResultT DefaultResultVal { get; set; }


    [MetaMember("mClassFlags")]
    public Flags ClassFlags { get; set; }

    [MetaMember("mClassIDs")]
    public HashSet<int> ClassIDs { get; set; }


    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }


    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumTestT>))]
    public struct EnumTestT
    {
        [MetaMember("mVal")]
        public Test Val { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumThresholdT>))]
    public struct EnumThresholdT
    {
        [MetaMember("mVal")]
        public Threshold Val { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumDefaultResultT>))]
    public struct EnumDefaultResultT
    {
        [MetaMember("mVal")]
        public DefaultResult Val { get; set; }
    }
}
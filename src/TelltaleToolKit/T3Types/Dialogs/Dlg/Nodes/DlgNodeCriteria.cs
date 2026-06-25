using System.Collections.Generic;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeCriteria>))]
public class DlgNodeCriteria : IDlgNode
{
    [MetaSerializer(typeof(EnumSerializer<DefaultResult>))]
    public enum DefaultResult
    {
        DefaultToPass = 1,
        DefaultToNotPass = 2,
        DefaultToNotPassUnlessTransparent = 3
    }

    [MetaSerializer(typeof(EnumSerializer<Test>))]
    public enum Test
    {
        Required = 1,
        Forbidden = 2
    }

    [MetaSerializer(typeof(EnumSerializer<Threshold>))]
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


    [MetaSerializer(typeof(MetaClassSerializer<EnumTestT>))]
    public struct EnumTestT
    {
        [MetaMember("mVal")]
        public Test Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumThresholdT>))]
    public struct EnumThresholdT
    {
        [MetaMember("mVal")]
        public Threshold Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumDefaultResultT>))]
    public struct EnumDefaultResultT
    {
        [MetaMember("mVal")]
        public DefaultResult Val { get; set; }
    }
}

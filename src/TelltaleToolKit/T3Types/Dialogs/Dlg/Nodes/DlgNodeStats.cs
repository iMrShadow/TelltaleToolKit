using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeStats>))]
public class DlgNodeStats
{
    [MetaSerializer(typeof(EnumSerializer<StatsType>))]
    public enum StatsType
    {
        Choices = 1,
        Extended = 2,
        CrowdPlay = 3,
        Relationships = 4
    }

    [MetaMember("mCohorts")]
    public DlgChildSetCohort Cohorts { get; set; }

    [MetaMember("mStatsType")]
    public StatsType StatsTypeValue { get; set; }

    [MetaMember("mhImage")]
    public Handle<T3Texture> Image { get; set; }

    [MetaMember("mDisplayText")]
    public LanguageResProxy DisplayText { get; set; }


    [MetaSerializer(typeof(MetaClassSerializer<Cohort>))]
    public class Cohort : IDlgChild
    {
        [MetaMember("mhImage")]
        public Handle<T3Texture> Image { get; set; }

        [MetaMember("mDisplayText1")]
        public LanguageResProxy DisplayText1 { get; set; }

        [MetaMember("mDisplayText2")]
        public LanguageResProxy DisplayText2 { get; set; }

        [MetaMember("mLayout")]
        public string Layout { get; set; }

        [MetaMember("mSummaryDisplayText")]
        public LanguageResProxy SummaryDisplayText { get; set; }

        [MetaMember("Baseclass_DlgChild")]
        public DlgChild DlgChild { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<DlgChildSetCohort>))]
    public class DlgChildSetCohort : IDlgChildSet
    {
        [MetaMember("Baseclass_DlgChildSet")]
        public DlgChildSet DlgChildSet { get; set; }
    }
}

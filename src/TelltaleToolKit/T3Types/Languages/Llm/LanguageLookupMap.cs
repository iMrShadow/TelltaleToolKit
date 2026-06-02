using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Languages.Llm;

[MetaSerializer(typeof(MetaClassSerializer<LanguageLookupMap>))]
public class LanguageLookupMap
{
    [MetaMember("mIDSets")]
    public List<DlgIdSet> IdSets { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<DlgIdSet>))]
    public class DlgIdSet
    {
        [MetaMember("mIDRange")]
        public Range<uint> IdRange { get; set; } = new();

        [MetaMember("mAdditionalIDs")]
        public HashSet<uint> AdditionalIDs { get; set; } = [];

        [MetaMember("mhDlg")]
        public Handle<Dlg> DlgReference { get; set; } = new();
    }
}

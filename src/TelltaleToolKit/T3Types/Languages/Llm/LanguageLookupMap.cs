using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Languages.Llm;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageLookupMap>))]
public class LanguageLookupMap
{
    [MetaMember("mIDSets")]
    public List<DlgIdSet> IdSets { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgIdSet>))]
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
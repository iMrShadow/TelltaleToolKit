using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Languages.Locreg;

namespace TelltaleToolKit.T3Types.Languages.Landb;
// New language database files

[MetaSerializer(typeof(Serializer))]
public class LanguageDb
{
    [MetaMember("Baseclass_UID::Owner")]
    public Owner UIdOwner { get; set; } = null!;

    [MetaMember("Baseclass_UID::Generator")]
    public Generator UIdGenerator { get; set; } = null!;

    [MetaMember("mLanguageResources")]
    public Dictionary<uint, LanguageRes> LanguageResources { get; set; } = new();

    [MetaMember("mRegistry")]
    public LocalizationRegistry Registry { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mProjectID")]
    public uint ProjectId { get; set; }

    [MetaMember("mExpandedIDRanges")]
    public List<ProjectDatabaseIdPair> ExpandedIdRanges { get; set; } = [];

    public class Serializer : MetaSerializer<LanguageDb>
    {
        private static readonly MetaClassSerializer<LanguageDb> s_metaClassSerializer = new();

        public override void Serialize(ref LanguageDb obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Read)
            {
                // There should be things in the debug section that I would like to parse.
                // It's not required, but it may contain useful information.
            }
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Languages.Llm;
using TelltaleToolKit.T3Types.Languages.Locreg;

namespace TelltaleToolKit.T3Types.Languages.Landb;

// New language database files

[MetaClassSerializerGlobal(typeof(Serializer))]
public class LanguageDb
{
    [MetaMember("Baseclass_UID::Owner")]
    public Owner UIdOwner { get; set; } = null!;

    [MetaMember("Baseclass_UID::Generator")]
    public Generator UIdGenerator { get; set; } = null!;

    [MetaMember("mLanguageResources")]
    public Dictionary<uint, LanguageRes> LanguageResources { get; set; } = [];

    [MetaMember("mRegistry")]
    public LocalizationRegistry Registry { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mProjectID")]
    public uint ProjectId { get; set; }

    [MetaMember("mExpandedIDRanges")]
    public List<ProjectDatabaseIdPair> ExpandedIdRanges { get; set; } = [];

    public class Serializer : MetaClassSerializer<LanguageDb>
    {
        private static readonly DefaultClassSerializer<LanguageDb> DefaultSerializer = new();

        public override void Serialize(ref LanguageDb obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamReader streamReader)
            {
                return;
                // There should be things in the debug section that I would like to parse.
                // It's not required, but it may contain useful information.
            }
        }
    }
}
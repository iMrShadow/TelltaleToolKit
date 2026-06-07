using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Locreg;

public class Localization
{
    [MetaSerializer(typeof(MetaClassSerializer<Language>))]
    public class Language
    {
        [MetaMember("mName")]
        public string Name { get; set; } = string.Empty;

        [MetaMember("mDisplayText")]
        public string DisplayText { get; set; } = string.Empty;

        [MetaMember("mAudioResourceSetName")]
        public string AudioResourceSetName { get; set; } = string.Empty;

        [MetaMember("mPlatformToSyncFSLocation")]
        public Dictionary<string, string> PlatformToSyncFSLocation { get; set; } = [];

        [MetaMember("mPlatformToSubgroupToAudioSyncFSLocation")]
        public Dictionary<string, Dictionary<string, string>>
            PlatformToSubgroupToAudioSyncFSLocation { get; set; } = [];

        [MetaMember("mSubgroupToResourceSetName")]
        public Dictionary<string, string> SubgroupToResourceSetName { get; set; } = [];

        [MetaMember("mFlags")]
        public Flags Flags { get; set; }

        [MetaMember("mIndex")]
        public uint Index { get; set; }

        [MetaMember("mVersionNumber")]
        public uint VersionNumber { get; set; }

        [MetaMember("mAudioVersionNumber")]
        public uint AudioVersionNumber { get; set; }
    }
}

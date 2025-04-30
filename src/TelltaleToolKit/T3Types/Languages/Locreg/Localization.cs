using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Locreg;

public class Localization
{
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Language>))]
    public class Language
    {
        [MetaMember("mName")]
        public string Name { get; set; }

        [MetaMember("mDisplayText")]
        public string DisplayText { get; set; }

        [MetaMember("mAudioResourceSetName")]
        public string AudioResourceSetName { get; set; }

        [MetaMember("mPlatformToSyncFSLocation")]
        public Dictionary<string, string> PlatformToSyncFSLocation { get; set; }

        [MetaMember("mPlatformToSubgroupToAudioSyncFSLocation")]
        public Dictionary<string, Dictionary<string, string>> PlatformToSubgroupToAudioSyncFSLocation { get; set; }

        [MetaMember("mSubgroupToResourceSetName")]
        public Dictionary<string, string> SubgroupToResourceSetName { get; set; }

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


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
    public LocalizationRegistry Registry { get; set; } = new();

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mProjectID")]
    public uint ProjectId { get; set; }

    [MetaMember("mExpandedIDRanges")]
    public List<ProjectDatabaseIdPair> ExpandedIdRanges { get; set; } = [];

    public class DebugMapping
    {
        public string InternalLanguageRes = string.Empty;
        public string Animation = string.Empty;
        public string VoiceSoundData = string.Empty;
    }

    public List<DebugMapping>? DebugFileMappings;
    public List<string>? DebugRegistrySymbolValueMappings;
    public List<string>? DebugRegistrySymbolValueMappingsReverse;

    public class Serializer : MetaSerializer<LanguageDb>
    {
        private static readonly MetaClassSerializer<LanguageDb> s_metaClassSerializer = new();

        public override void PreSerialize(ref LanguageDb? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new LanguageDb();
        }

        public override void Serialize(ref LanguageDb obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Params.StreamVersion >= 5 && stream.BeginDebugSection())
            {
                if (stream.Mode is MetaStreamMode.Write)
                {
                    if (obj.DebugFileMappings != null)
                    {
                        foreach (var list in obj.DebugFileMappings)
                        {
                            stream.Write(list.InternalLanguageRes);
                            stream.Write(list.Animation);
                            stream.Write(list.VoiceSoundData);
                        }
                    }

                    if (obj.DebugRegistrySymbolValueMappings != null)
                    {
                        foreach (string valueMapping in obj.DebugRegistrySymbolValueMappings)
                        {
                            stream.Write(valueMapping);
                        }
                    }

                    if (obj.DebugRegistrySymbolValueMappingsReverse != null)
                    {
                        foreach (string valueMapping in obj.DebugRegistrySymbolValueMappingsReverse)
                        {
                            stream.Write(valueMapping);
                        }
                    }
                }
                else if (stream.Mode is MetaStreamMode.Read)
                {
                    int num = obj.LanguageResources.Count;

                    obj.DebugFileMappings = new List<DebugMapping>(num);
                    for (int i = 0; i < num; i++)
                    {
                        DebugMapping mapping = new()
                        {
                            InternalLanguageRes = stream.ReadString(),
                            Animation = stream.ReadString(),
                            VoiceSoundData = stream.ReadString()
                        };
                        obj.DebugFileMappings.Add(mapping);
                    }

                    num = obj.Registry.FlagIndexMap.Count;
                    obj.DebugRegistrySymbolValueMappings = new List<string>(num);
                    for (int i = 0; i < num; i++)
                    {
                        obj.DebugRegistrySymbolValueMappings.Add(stream.ReadString());
                    }

                    num = obj.Registry.FlagIndexMapReverse.Count;
                    obj.DebugRegistrySymbolValueMappingsReverse = new List<string>(num);
                    for (int i = 0; i < num; i++)
                    {
                        obj.DebugRegistrySymbolValueMappingsReverse.Add(stream.ReadString());
                    }
                }

                stream.EndDebugSection();
            }
        }
    }
}

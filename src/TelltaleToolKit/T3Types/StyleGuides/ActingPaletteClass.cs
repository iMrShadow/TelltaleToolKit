using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(Serializer))]
public class ActingPaletteClass
{
    public List<ActingAccentPalette> AccentPalettePtrs = [];
    public List<ActingPaletteGroup> PaletteGroupPtrs = [];

    public List<ActingPalette> PalletePtrs = [];

    [MetaMember("Baseclass_UID::Owner")]
    public Owner BaseClassOwner { get; set; } = new();

    [MetaMember("Baseclass_UID::Generator")]
    public Generator BaseClassGenerator { get; set; } = new();

    [MetaMember("Baseclass_ActingOverridablePropOwner")]
    public ActingOverridablePropOwner BaseClassActingOverridablePropOwner { get; set; } = new();

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mKeywords")]
    public List<string> Keywords { get; set; } = [];

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mInstantChange")]
    public bool InstantChange { get; set; }

    [MetaMember("mPalettes")]
    public List<ActingPalette> Palettes { get; set; } = [];

    [MetaMember("mPaletteGroups")]
    public List<ActingPaletteGroup> PaletteGroups { get; set; } = [];

    [MetaMember("mAlternateNames")]
    public List<string> AlternateNames { get; set; } = [];

    [MetaMember("mDefaultPaletteGroupID")]
    public int DefaultPaletteGroupId { get; set; }

    [MetaMember("mIdle")]
    public AnimOrChore Idle { get; set; } = new();

    public class Serializer : MetaSerializer<ActingPaletteClass>
    {
        private static readonly MetaClassSerializer<ActingPaletteClass> s_metaClassSerializer = new();

        public override void Serialize(ref ActingPaletteClass obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write && obj.AccentPalettePtrs.Count > 0)
            {
                obj.Flags.Set(3);
            }

            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            // Enable?
            obj.Flags.Set(1);

            MetaClass? classDescription = stream.GetMetaClass(typeof(ActingPaletteClass));

            if (classDescription is not null && classDescription.ContainsMember("mFlags"))
            {
                if (stream.Mode is MetaStreamMode.Write)
                {
                    stream.Write(obj.PalletePtrs.Count);
                    foreach (ActingPalette palette in obj.PalletePtrs)
                    {
                        ActingPalette actingPalette = palette;
                        stream.Serialize(ref actingPalette);
                    }

                    if (obj.Flags.Has(2))
                    {
                        stream.Write(obj.AccentPalettePtrs.Count);
                        foreach (ActingAccentPalette accentPalette in obj.AccentPalettePtrs)
                        {
                            ActingAccentPalette actingAccentPalette = accentPalette;
                            stream.Serialize(ref actingAccentPalette);
                        }
                    }

                    stream.Write(obj.PaletteGroupPtrs.Count);
                    foreach (ActingPaletteGroup paletteGroup in obj.PaletteGroupPtrs)
                    {
                        ActingPaletteGroup actingPaletteGroup = paletteGroup;
                        stream.Serialize(ref actingPaletteGroup);
                    }
                }
                else
                {
                    int count = stream.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        ActingPalette palette = new();
                        // Generate Unique ID
                        stream.Serialize(ref palette);
                        obj.PalletePtrs.Add(palette);
                    }

                    if (obj.Flags.Has(2))
                    {
                        int accentPaletteCount = stream.ReadInt32();
                        for (int i = 0; i < accentPaletteCount; i++)
                        {
                            ActingAccentPalette palette = new();
                            // Generate Unique ID
                            stream.Serialize(ref palette);
                            obj.AccentPalettePtrs.Add(palette);
                        }
                    }

                    int groupCount = stream.ReadInt32();

                    for (int i = 0; i < groupCount; i++)
                    {
                        ActingPaletteGroup group = new();
                        // Generate Unique ID
                        stream.Serialize(ref group);
                        obj.PaletteGroupPtrs.Add(group);
                    }
                }
            }
        }
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(Serializer))]
public class ActingPaletteClass
{
    [MetaMember("Baseclass_UID::Owner")]
    public Owner BaseClassOwner { get; set; }

    [MetaMember("Baseclass_UID::Generator")]
    public Generator BaseClassGenerator { get; set; }

    [MetaMember("Baseclass_ActingOverridablePropOwner")]
    public ActingOverridablePropOwner BaseClassActingOverridablePropOwner { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mKeywords")]
    public List<string> Keywords { get; set; } = [];

    [MetaMember("mPalettes")]
    public List<ActingPalette> Palettes { get; set; } = [];

    [MetaMember("mPaletteGroups")]
    public List<ActingPaletteGroup> PaletteGroups { get; set; } = [];

    [MetaMember("mAlternateNames")]
    public List<string> AlternateNames { get; set; } = [];

    [MetaMember("mDefaultPaletteGroupID")]
    public int DefaultPaletteGroupID { get; set; }

    [MetaMember("mIdle")]
    public AnimOrChore Idle { get; set; }

    public class Serializer : MetaSerializer<ActingPaletteClass>
    {
        private static readonly MetaClassSerializer<ActingPaletteClass> s_metaClassSerializer = new();

        public override void Serialize(ref ActingPaletteClass obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            // TODO:
            MetaClass? classDescription = stream.GetMetaClass(typeof(ActingPaletteClass));

            if (classDescription is not null && classDescription.ContainsMember("mFlags"))
            {
                if (stream.Mode is MetaStreamMode.Write)
                {
                    throw new NotImplementedException();
                }

                if (stream.Mode is MetaStreamMode.Read)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

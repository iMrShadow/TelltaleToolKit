using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(Serializer))]
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

    public class Serializer : MetaClassSerializer<ActingPaletteClass>
    {
        private static readonly DefaultClassSerializer<ActingPaletteClass> DefaultSerializer = new();

        public override void Serialize(ref ActingPaletteClass obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            // TODO:
            MetaClass? classDescription = stream.GetMetaClass(typeof(ActingPaletteClass));

            if (classDescription is not null && classDescription.ContainsMember("mFlags"))
            {
                if (stream is MetaStreamWriter streamWriter)
                {
                    throw new NotImplementedException();
                }
                else if (stream is MetaStreamReader streamReader)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
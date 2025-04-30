using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class StyleGuide
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator BaseclassUidGenerator { get; set; } = new();

    [MetaMember("Baseclass_ActingOverridablePropOwner")]
    public ActingOverridablePropOwner BaseclassActingOverridablePropOwner { get; set; } = new();

    [MetaMember("mDefPaletteClassID")]
    public int DefPaletteClassId { get; set; }

    [MetaMember("mDefPaletteClassIndex")]
    public int DefPaletteClassIndex { get; set; }

    [MetaMember("mPaletteClasses")]
    public List<ActingPaletteClass> PaletteClasses { get; set; } = [];

    [MetaMember("mbGeneratesLookAts")]
    public bool GeneratesLookAts { get; set; } = false;

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }


    // It's a metamember, but not serialized by default. TODO:
    public List<ActingPaletteClass> PaletteClassesPtrs { get; set; } = [];


    [MetaClassSerializerGlobal(typeof(ActingOverridablePropOwnerSerializer))]
    public class Serializer : MetaClassSerializer<StyleGuide>
    {
        private static readonly DefaultClassSerializer<StyleGuide> DefaultSerializer = new();

        public override void Serialize(ref StyleGuide obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            MetaClass? classDescription = stream.GetMetaClass(typeof(StyleGuide));

            if (classDescription is not null && classDescription.ContainsMember("mFlags"))
            {
                if (stream is MetaStreamWriter streamWriter)
                {
                    throw new NotImplementedException();
                }
                else if (stream is MetaStreamReader streamReader)
                {
                    int values = streamReader.ReadInt32();

                    for (var i = 0; i < values; i++)
                    {
                        var child = new ActingPaletteClass();
                        TTK.PreSerialize<ActingPaletteClass>(ref child, stream);
                        TTK.Serialize<ActingPaletteClass>(ref child, stream);
                        obj.PaletteClassesPtrs.Add(child);
                    }
                }
            }
        }
    }
}
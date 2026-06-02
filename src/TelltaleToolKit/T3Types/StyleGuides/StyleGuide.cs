using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(Serializer))]
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


    [MetaSerializer(typeof(ActingOverridablePropOwnerSerializer))]
    public class Serializer : MetaSerializer<StyleGuide>
    {
        private static readonly MetaClassSerializer<StyleGuide> s_metaClassSerializer = new();

        public override void Serialize(ref StyleGuide obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            MetaClass? classDescription = stream.GetMetaClass(typeof(StyleGuide));

            if (classDescription is not null && classDescription.ContainsMember("mFlags"))
            {
                if (stream.Mode is MetaStreamMode.Write)
                {
                    throw new NotImplementedException();
                }

                if (stream.Mode is MetaStreamMode.Read)
                {
                    int values = stream.ReadInt32();

                    for (var i = 0; i < values; i++)
                    {
                        var child = new ActingPaletteClass();
                        stream.Serialize<ActingPaletteClass>(ref child);
                        obj.PaletteClassesPtrs.Add(child);
                    }
                }
            }
        }
    }
}

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

    [MetaMember("mbDisallowOverrun")]
    public bool DisallowOverrun { get; set; } = false;

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mPaletteClassPtrs")]
    public List<ActingPaletteClass> PaletteClassesPtrs { get; set; } = [];

    public class Serializer : MetaSerializer<StyleGuide>
    {
        private static readonly MetaClassSerializer<StyleGuide> s_metaClassSerializer = new();

        public override void Serialize(ref StyleGuide obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            MetaClass? classDescription = stream.GetMetaClass(typeof(StyleGuide));

            if (classDescription is null || !classDescription.ContainsMember("mFlags"))
            {
                return;
            }

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.PaletteClassesPtrs.Count);
                foreach (ActingPaletteClass paletteClass in obj.PaletteClassesPtrs)
                {
                    ActingPaletteClass actingPaletteClass = paletteClass;
                    stream.Serialize(ref actingPaletteClass);
                }
            }
            else
            {
                obj.PaletteClassesPtrs = [];

                int values = stream.ReadInt32();
                for (int i = 0; i < values; i++)
                {
                    ActingPaletteClass child = new();
                    stream.Serialize(ref child);
                    obj.PaletteClassesPtrs.Add(child);
                }
            }
        }
    }
}

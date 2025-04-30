using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<StyleGuideRef>))]
public class StyleGuideRef
{
    [MetaMember("mhStyleGuide")]
    public Handle<StyleGuide> StyleGuide { get; set; } = new();

    [MetaMember("mPaletteClassIndex")]
    public int PaletteClassIndex { get; set; }

    [MetaMember("mPalettesUsed")]
    public List<bool> PalettesUsed { get; set; } = [];

    [MetaMember("mOverridden")]
    public bool Overridden { get; set; }
}
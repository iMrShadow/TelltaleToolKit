using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(MetaClassSerializer<StyleGuideRef>))]
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

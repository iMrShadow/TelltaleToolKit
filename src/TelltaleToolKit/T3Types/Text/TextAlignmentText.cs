using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Text;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TextAlignmentType>))]
public class TextAlignmentType
{
    [MetaMember("mAlignmentType")]
    public int AlignmentType { get; set; }
}

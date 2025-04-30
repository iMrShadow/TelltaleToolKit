using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Text;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TextAlignmentType>))]
public class TextAlignmentType
{
    [MetaMember("mAlignmentType")]
    public int AlignmentType { get; set; }
}
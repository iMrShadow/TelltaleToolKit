using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Languages.Lanreg;

[MetaSerializer(typeof(MetaClassSerializer<LanguageRegister>))]
public class LanguageRegister
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator UIDGenerator { get; set; } = new();
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Languages.Lanreg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageRegister>))]
public class LanguageRegister
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator UIDGenerator { get; set; } = null!;
}
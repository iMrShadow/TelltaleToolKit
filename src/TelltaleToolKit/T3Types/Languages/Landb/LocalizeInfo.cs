using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LocalizeInfo>))]
public class LocalizeInfo
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}
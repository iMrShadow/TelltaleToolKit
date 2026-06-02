using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LocalizeInfo>))]
public class LocalizeInfo
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}

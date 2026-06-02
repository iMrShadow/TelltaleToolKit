using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.DlgSettings;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgSystemSettings>))]
public class DlgSystemSettings
{
    [MetaMember("mPropsMapUser")]
    public DlgObjectPropsMap PropsMapUser { get; set; }

    [MetaMember("mPropsMapProduction")]
    public DlgObjectPropsMap PropsMapProduction { get; set; }
}

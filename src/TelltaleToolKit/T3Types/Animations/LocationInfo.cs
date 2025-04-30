using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Skeletons;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LocationInfo>))]
public class LocationInfo
{
    [MetaMember("mAttachmentAgent")]
    public string AttachmentAgent { get; set; } = string.Empty;

    [MetaMember("mAttachmentNode")]
    public Symbol AttachmentNode { get; set; }

    [MetaMember("mInitialLocalTransform")]
    public Transform InitialLocalTransform { get; set; } = new();
}
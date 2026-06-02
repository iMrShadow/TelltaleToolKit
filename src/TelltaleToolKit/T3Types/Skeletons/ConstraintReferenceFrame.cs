using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(EnumSerializer<ConstraintReferenceFrame>))]
public enum ConstraintReferenceFrame {
    World = 0,
    Root = 1,
    Parent = 2,
}

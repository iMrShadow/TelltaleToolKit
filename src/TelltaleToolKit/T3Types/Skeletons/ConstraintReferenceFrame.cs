using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(EnumSerializer<ConstraintReferenceFrame>))]
public enum ConstraintReferenceFrame {
    World = 0,
    Root = 1,
    Parent = 2,
}
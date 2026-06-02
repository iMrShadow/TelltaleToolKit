using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumVTextAlignmentType>))]
public struct EnumVTextAlignmentType
{
    [MetaMember("mVal")]
    public VTextAlignmentType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<VTextAlignmentType>))]
public enum VTextAlignmentType
{
    //  eVTextAlignment_
    None = 0,
    Top = 1,
    Middle = 2,
    Bottom = 3,
}

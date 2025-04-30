using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<BlendMode>))]
public struct BlendMode
{
    [MetaMember("mVal")]
    public T3BlendMode Val {get;set;}
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3BlendMode>))]
public enum T3BlendMode
{
    // eBlendMode_
    Default = -1,
    Normal = 0,
    Alpha = 1,
    AlphaAlphaTest = 2,
    AlphaTest = 3,
    InvAlphaTest = 4,
    Add = 5,
    Multiply = 6,
    InvMultiply = 7,
    AlphaAdd = 8,
    AlphaSubtract = 9,
    AlphaInvAlphaTest = 10,
    AddAlphaTest = 11,
    AddInvAlphaTest = 12,
    MultiplyAlphaTest = 13,
    MultiplyInvAlphaTest = 14,
}
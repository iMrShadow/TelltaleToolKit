using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumRenderMaskTest>))]
public struct EnumRenderMaskTest
{
    [MetaMember("mVal")]
    public RenderMaskTest Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<RenderMaskTest>))]
public enum RenderMaskTest
{
    //        eRenderMaskTest_
    None = 0x1,
    Set = 0x2,
    Clear = 0x3,
}
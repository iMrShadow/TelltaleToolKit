using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3MaterialLightModelType>))]
public struct EnumT3MaterialLightModelType
{
    [MetaMember("mVal")]
    public T3MaterialLightModelType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialLightModelType>))]
public enum T3MaterialLightModelType
{
    // eMaterialLightModel_
    Default = -1,
    Unlit = 0,
    VertexDiffuse = 1,
    Diffuse = 2,
    Phong = 3,
    PhongGloss = 4,
    Toon = 5,
    NPR_Depreceated = 6,
    PBS = 7,
    Cloth = 8,
    Hair = 9,
    Skin = 0xA,
    HybridCloth = 0xB,
    DiffuseNoDirection = 0xC,
    HybridHair = 0xD,
    Count = 0xE,
}
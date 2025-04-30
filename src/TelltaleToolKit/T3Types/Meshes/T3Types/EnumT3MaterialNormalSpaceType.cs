using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3MaterialNormalSpaceType>))]
public struct EnumT3MaterialNormalSpaceType
{
    [MetaMember("mVal")]
    public T3MaterialNormalSpaceType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialNormalSpaceType>))]
public enum T3MaterialNormalSpaceType
{
    //  eMaterialNormalSpace_
    Tangent = 0x0,
    World = 0x1,
}
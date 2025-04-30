using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3MaterialLODFullyRough>))]
public struct EnumT3MaterialLODFullyRough
{
    [MetaMember("mVal")]
    public T3MaterialLODFullyRough Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialLODFullyRough>))]
public enum T3MaterialLODFullyRough
{
    // TODO:
}

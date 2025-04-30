using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialTextureParam>))]
public class T3MaterialTextureParam
{
    [MetaMember("mParamType")]
    public T3MaterialTextureParamType ParamType { get; set; }

    [MetaMember("mValueType")]
    public T3MaterialValueType ValueType { get; set; }

    [MetaMember("mFlags")]
    public uint Flags { get; set; }

    [MetaMember("mScalarOffset")]
    public int ScalarOffset { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialTextureParamType>))]
public enum T3MaterialTextureParamType
{
    //  eMaterialTextureParam_
    None = -1,
    Unused = 0,
}
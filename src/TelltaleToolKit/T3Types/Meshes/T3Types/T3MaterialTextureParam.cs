using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MaterialTextureParam>))]
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

[MetaSerializer(typeof(EnumSerializer<T3MaterialTextureParamType>))]
public enum T3MaterialTextureParamType
{
    //  eMaterialTextureParam_
    None = -1,
    Unused = 0,
}

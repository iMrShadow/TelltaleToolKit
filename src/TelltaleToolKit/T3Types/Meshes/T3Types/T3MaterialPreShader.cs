using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialPreShader>))]
public class T3MaterialPreShader
{
    [MetaMember("mValueType")]
    public T3MaterialValueType ValueType { get; set; }

    [MetaMember("mFlags")]
    public uint Flags { get; set; }

    [MetaMember("mPreShaderOffset")]
    public int PreShaderOffset { get; set; }

    [MetaMember("mScalarParameterOffset")]
    public int ScalarParameterOffset { get; set; }

    [MetaMember("mScalarParameterOffset[0]")]
    public int ScalarParameterOffset0 { get; set; }

    [MetaMember("mScalarParameterOffset[1]")]
    public int ScalarParameterOffset1 { get; set; }

    [MetaMember("mScalarParameterOffset[2]")]
    public int ScalarParameterOffset2 { get; set; }

    [MetaMember("mScalarParameterOffset[3]")]
    public int ScalarParameterOffset3 { get; set; }
}
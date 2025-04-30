using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

// TODO: Handle arrays properly.
// I am probably going to add SArrays (unfortunately)
// Maybe check if the value contains "[...]"?
// Or maybe leave it as it is.
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialTransform2D>))]
public class T3MaterialTransform2D
{
    [MetaMember("mParameterPrefix")]
    public Symbol ParameterPrefix { get; set; }

    [MetaMember("mFlags")]
    public uint Flags { get; set; }

    [MetaMember("mScalarOffset0")]
    public int ScalarOffset0 { get; set; }

    [MetaMember("mScalarOffset1")]
    public int ScalarOffset1 { get; set; } 
    
    [MetaMember("mScalarOffset0[0]")]
    public int ScalarOffset00 { get; set; }

    [MetaMember("mScalarOffset1[0]")]
    public int ScalarOffset10 { get; set; }  
    
    [MetaMember("mScalarOffset0[1]")]
    public int ScalarOffset01 { get; set; }

    [MetaMember("mScalarOffset1[1]")]
    public int ScalarOffset11 { get; set; } 

    [MetaMember("mPreShaderScalarOffset0")]
    public int PreShaderScalarOffset0 { get; set; }

    [MetaMember("mPreShaderScalarOffset1")]
    public int PreShaderScalarOffset1 { get; set; }

    [MetaMember("mNestedMaterialIndex")]
    public int NestedMaterialIndex { get; set; }
}
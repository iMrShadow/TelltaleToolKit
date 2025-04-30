using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.LightMaps;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class T3MaterialData
{
    [MetaMember("mMaterialName")]
    public Symbol MaterialName { get; set; }

    [MetaMember("mRuntimePropertiesName")]
    public Symbol RuntimePropertiesName { get; set; }

    [MetaMember("mLegacyRenderTextureProperty")]
    public Symbol LegacyBlendModeTextureProperty { get; set; }
    
    [MetaMember("mLegacyBlendModeRuntimeProperty")]
    public Symbol LegacyBlendModeRuntimeProperty { get; set; }

    [MetaMember("mDomain")]
    public T3MaterialDomainType Domain { get; set; } //wd4+ TODO:

    [MetaMember("mLightType")]
    public LightType LightType { get; set; } //bat2 and below

    [MetaMember("mRuntimeProperties")]
    public List<T3MaterialRuntimeProperty> RuntimeProperties { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mMaxDistance")]
    public float MaxDistance { get; set; } //bat2 and below, not sure if its a float/int always 0

    [MetaMember("mCompiledData2")]
    public List<T3MaterialCompiledData> CompiledData2 { get; set; }

    public class Serializer : MetaClassSerializer<T3MaterialData>
    {
        private static readonly DefaultClassSerializer<T3MaterialData> DefaultSerializer = new();

        public override void PreSerialize(ref T3MaterialData obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new T3MaterialData();
            }
        }

        public override void Serialize(ref T3MaterialData obj, MetaStream stream)
        {
            DefaultSerializer.Serialize(ref obj, stream);

            MetaClass? classDescription = stream.GetMetaClass(typeof(T3MaterialData));

            if (classDescription == null || classDescription.ContainsMember("mCompiledData2"))
                return;

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int numCompiledData = streamReader.ReadInt32();

                obj.CompiledData2 = new List<T3MaterialCompiledData>(numCompiledData);

                for (var i = 0; i < numCompiledData; i++)
                {
                    // TODO: Add the index to the main class
                    int materialIndex = streamReader.ReadInt32();

                    T3MaterialCompiledData compiledData = new();
                    TTKContext.Instance().GetSerializer<T3MaterialCompiledData>().Serialize(ref compiledData, stream);
                    obj.CompiledData2.Add(compiledData);
                }
            }
        }
    }
}
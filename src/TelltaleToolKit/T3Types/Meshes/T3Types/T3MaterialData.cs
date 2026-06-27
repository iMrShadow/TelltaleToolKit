using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.LightMaps;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(Serializer))]
public class T3MaterialData
{
    [MetaMember("mMaterialName")]
    public Symbol MaterialName { get; set; } = Symbol.Empty;

    [MetaMember("mRuntimePropertiesName")]
    public Symbol RuntimePropertiesName { get; set; } = Symbol.Empty;

    [MetaMember("mLegacyRenderTextureProperty")]
    public Symbol LegacyBlendModeTextureProperty { get; set; } = Symbol.Empty;

    [MetaMember("mLegacyBlendModeRuntimeProperty")]
    public Symbol LegacyBlendModeRuntimeProperty { get; set; } = Symbol.Empty;

    [MetaMember("mDomain")]
    public T3MaterialDomainType Domain { get; set; } //wd4+

    [MetaMember("mLightType")]
    public LightType LightType { get; set; } //bat2 and below

    [MetaMember("mRuntimeProperties")]
    public List<T3MaterialRuntimeProperty> RuntimeProperties { get; set; } = [];

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mMaxDistance")]
    public float MaxDistance { get; set; } //bat2 and below, not sure if its a float/int always 0

    [MetaMember("mCompiledData2")]
    public List<T3MaterialCompiledData> CompiledData2 { get; set; } = [];

    public class Serializer : MetaSerializer<T3MaterialData>
    {
        private static readonly MetaClassSerializer<T3MaterialData> s_metaClassSerializer = new();

        public override void PreSerialize(ref T3MaterialData? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new T3MaterialData();
        }

        public override void Serialize(ref T3MaterialData obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.Serialize(ref obj, stream);

            MetaClass? classDescription = stream.GetMetaClass(typeof(T3MaterialData));

            if (classDescription == null || classDescription.ContainsMember("mCompiledData2"))
                return;

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.CompiledData2.Count);

                for (int i = 0; i < obj.CompiledData2.Count; i++)
                {
                    stream.Write(i);

                    T3MaterialCompiledData compiledData = obj.CompiledData2[i];

                    stream.Serialize(ref compiledData);
                    obj.CompiledData2[i] = compiledData;
                }
            }
            else
            {
                int numCompiledData = stream.ReadInt32();

                obj.CompiledData2 = new List<T3MaterialCompiledData>(numCompiledData);

                for (int i = 0; i < numCompiledData; i++)
                    obj.CompiledData2.Add(new T3MaterialCompiledData());

                for (int i = 0; i < numCompiledData; i++)
                {
                    int materialIndex = stream.ReadInt32();

                    if ((uint)materialIndex >= (uint)numCompiledData)
                        throw new InvalidDataException(
                            $"Material index {materialIndex} was outside the valid range 0..{numCompiledData - 1}.");

                    T3MaterialCompiledData compiledData = obj.CompiledData2[materialIndex];
                    stream.Serialize(ref compiledData);

                    obj.CompiledData2[materialIndex] = compiledData;
                }
            }
        }
    }
}

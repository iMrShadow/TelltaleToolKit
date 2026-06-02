using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<AnimOrChore>))]
public class EnlightenModule
{
    [MetaSerializer(typeof(MetaClassSerializer<EnlightenSystemSettings>))]
    public class EnlightenSystemSettings
    {
        [MetaMember("mDefaultQuality")]
        public EnumeQuality DefaultQuality { get; set; }

        [MetaMember("mAdaptiveProbeResolution")]
        public EnumeProbeResolution AdaptiveProbeResolution { get; set; }

        [MetaMember("mDisableEnlighten")]
        public bool DisableEnlighten { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeProbeResolution>))]
    public struct EnumeProbeResolution
    {
        [MetaMember("mVal")]
        public ProbeResolution Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenCubemapSettings>))]
    public class EnlightenCubemapSettings
    {
        [MetaMember("mFaceWidth")]
        public int FaceWidth { get; set; }

        [MetaMember("mBoxOrigin")]
        public Vector3 BoxOrigin { get; set; }
    }

    public struct EnlightenAdaptiveProbeVolumeSettings
    {
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeQuality>))]
    public struct EnumeQuality
    {
        [MetaMember("mVal")]
        public Quality Val { get; set; }
    }

    public struct EnlightenProbeVolumeSettings
    {
    }

    public struct EnlightenAutoProbeVolumeSettings
    {
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenLightSettings>))]
    public struct EnlightenLightSettings
    {
        [MetaMember("mEnlightenSaturation")]
        public float EnlightenSaturation { get; set; }

        [MetaMember("mEnlightenOnly")]
        public bool EnlightenOnly { get; set; }

        [MetaMember("mCastDynamicEnlightenShadows")]
        public bool CastDynamicEnlightenShadows { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenPrimitiveSettings>))]
    public class EnlightenPrimitiveSettings
    {
        [MetaMember("mSystemId")]
        public string SystemId { get; set; } = string.Empty;

        [MetaMember("mEnlightenLightingMode")]
        public EnumeInstanceType EnlightenLightingMode { get; set; }

        [MetaMember("mEnlightenUpdateMethod")]
        public EnumeUpdateMethod EnlightenUpdateMethod { get; set; }

        [MetaMember("mEnlightenQuality")]
        public EnumeQualityWithDefault EnlightenQuality { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeInstanceType>))]
    public struct EnumeInstanceType
    {
        [MetaMember("mVal")]
        public InstanceType Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeUpdateMethod>))]
    public struct EnumeUpdateMethod
    {
        [MetaMember("mVal")]
        public UpdateMethod Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeQualityWithDefault>))]
    public struct EnumeQualityWithDefault
    {
        [MetaMember("mVal")]
        public QualityWithDefault Val { get; set; }
    }

    public struct EnumeDistributedBuildSystem
    {
    }

    public struct EnumeSceneOptimisationMode
    {
    }

    public struct EnumeBackfaceType
    {
    }

    public struct EnumeAutoUVSimplificationMode
    {
    }

    public struct EnumeProbeSampleMethod
    {
    }

    public struct EnumeDisplayQuality
    {
    }

    public struct EnumeRadiositySampleRate
    {
    }

    public struct EnumeAgentUsage
    {
    }

    public struct EnumeUpdateMethodWithDefault
    {
    }

    public struct EnumeProbeResolutionWithDefault
    {
    }

    public struct EnlightenMeshSettings
    {
        public class AutoUVSettings
        {
        }
    }

    public struct EnumeSimplifyMode
    {
    }

    [MetaSerializer(typeof(EnumSerializer<InstanceType>))]
    public enum InstanceType
    {
        //  InstanceType_
        Radiosity = 0x0,
        StaticSetDressing_Unused = 0x1,
        FullyDynamic = 0x2,
        ProbeRadiosity = 0x3,
        Auto = 0x4,
        Disabled = 0x5,
    }

    [MetaSerializer(typeof(EnumSerializer<UpdateMethod>))]
    public enum UpdateMethod
    {
        // UpdateMethod_
        Dynamic = 0x0,
        Static = 0x1,
        Auto = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<QualityWithDefault>))]
    public enum QualityWithDefault
    {
        //  QualityWithDefault_
        Background = 0x0,
        Low = 0x1,
        Medium = 0x2,
        High = 0x3,
        Auto = 0x4,
        UseDefault = 0x5,
    }

    [MetaSerializer(typeof(EnumSerializer<ProbeResolution>))]
    public enum ProbeResolution
    {
        //    ProbeResolution_
        Full = 0x0,
        ProbeResolution_1_2 = 0x1,
        ProbeResolution_1_4 = 0x2,
        ProbeResolution_1_8 = 0x3,
    }

    [MetaSerializer(typeof(EnumSerializer<Quality>))]
    public enum Quality
    {
        //     Quality_
        Background = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Auto = 4,
    }
}

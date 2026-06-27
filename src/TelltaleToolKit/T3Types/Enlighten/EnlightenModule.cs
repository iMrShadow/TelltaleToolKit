using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Enlighten;

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

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenAdaptiveProbeVolumeSettings>))]
    public struct EnlightenAdaptiveProbeVolumeSettings
    {
        [MetaMember("mQuality")]
        public EnumeQuality QualityE { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeQuality>))]
    public struct EnumeQuality
    {
        [MetaMember("mVal")]
        public Quality Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenProbeVolumeSettings>))]
    public class EnlightenProbeVolumeSettings
    {
        [MetaMember("mQuality")]
        public EnumeQuality QualityE { get; set; }= new();

        [MetaMember("mLightmapType")]
        public string LightmapType { get; set; } = string.Empty;

        [MetaMember("mResolution")]
        public Vector3 Resolution { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenAutoProbeVolumeSettings>))]
    public struct EnlightenAutoProbeVolumeSettings
    {
        [MetaMember("mQuality")]
        public EnumeQuality QualityE { get; set; }

        [MetaMember("mMinProbeSetSize")]
        public int MinProbeSetSize { get; set; }

        [MetaMember("mMergingThreshold")]
        public float MergingThreshold { get; set; }

        [MetaMember("mMinProbeSpacing")]
        public int MinProbeSpacing { get; set; }

        [MetaMember("mMaxProbeSpacing")]
        public int MaxProbeSpacing { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenLightSettings>))]
    public class EnlightenLightSettings
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

    [MetaSerializer(typeof(MetaClassSerializer<EnumeDistributedBuildSystem>))]
    public struct EnumeDistributedBuildSystem
    {
        [MetaMember("mVal")]
        public DistributedBuildSystem Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeSceneOptimisationMode>))]
    public struct EnumeSceneOptimisationMode
    {
        [MetaMember("mVal")]
        public SceneOptimisationMode Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeBackfaceType>))]
    public struct EnumeBackfaceType
    {
        [MetaMember("mVal")]
        public BackfaceType Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeAutoUVSimplificationMode>))]
    public struct EnumeAutoUVSimplificationMode
    {
        [MetaMember("mVal")]
        public AutoUVSimplificationMode Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeProbeSampleMethod>))]
    public struct EnumeProbeSampleMethod
    {
        [MetaMember("mVal")]
        public ProbeSampleMethod Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeDisplayQuality>))]
    public struct EnumeDisplayQuality
    {
        [MetaMember("mVal")]
        public DisplayQuality Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeRadiositySampleRate>))]
    public struct EnumeRadiositySampleRate
    {
        [MetaMember("mVal")]
        public RadiositySampleRate Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeAgentUsage>))]
    public struct EnumeAgentUsage
    {
        [MetaMember("mVal")]
        public AgentUsage Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeUpdateMethodWithDefault>))]
    public struct EnumeUpdateMethodWithDefault
    {
        [MetaMember("mVal")]
        public UpdateMethodWithDefault Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeProbeResolutionWithDefault>))]
    public struct EnumeProbeResolutionWithDefault
    {
        [MetaMember("mVal")]
        public ProbeResolutionWithDefault Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnlightenMeshSettings>))]
    public class EnlightenMeshSettings
    {
        [MetaMember("mEnlightenLightingMode")]
        public EnumeInstanceType EnlightenLightingMode { get; set; }

        [MetaMember("mAutoUVSettings")]
        public AutoUVSettings AutoUvSettings { get; set; } = new();

        [MetaMember("mEnlightenQuality")]
        public EnumeQuality EnlightenQuality { get; set; }

        [MetaSerializer(typeof(MetaClassSerializer<AutoUVSettings>))]
        public class AutoUVSettings
        {
            [MetaMember("mSimplificationMode")]
            public EnumeSimplifyMode SimplificationMode { get; set; }

            [MetaMember("mMaxDistance")]
            public float MaxDistance { get; set; }

            [MetaMember("mMaxInitialNormalDeviation")]
            public float MaxInitialNormalDeviation { get; set; }

            [MetaMember("mMaxGeneralNormalDeviation")]
            public float MaxGeneralNormalDeviation { get; set; }

            [MetaMember("mExpansionFactor")]
            public float ExpansionFactor { get; set; }

            [MetaMember("mSignificantAreaRatio")]
            public float SignificantAreaRatio { get; set; }
        }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumeSimplifyMode>))]
    public struct EnumeSimplifyMode
    {
        [MetaMember("mVal")]
        public SimplifyMode Val { get; set; }
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

    [MetaSerializer(typeof(EnumSerializer<DistributedBuildSystem>))]
    public enum DistributedBuildSystem
    {
        None = 0x0,
        SN_DBS = 0x1,
        Incredibuild = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<SceneOptimisationMode>))]
    public enum SceneOptimisationMode
    {
        None = 0x0,
        EqualPixelArea = 0x1,
        Voxelisation = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<BackfaceType>))]
    public enum BackfaceType
    {
        EBT_Auto = 0x0,
        EBT_Invalid = 0x1,
        EBT_Black = 0x2,
        EBT_Transparent = 0x3,
        EBT_Double_Sided = 0x4,
    }

    [MetaSerializer(typeof(EnumSerializer<AutoUVSimplificationMode>))]
    public enum AutoUVSimplificationMode
    {
        EAUM_Automatic = 0x0,
        EAUM_Override = 0x1,
        EAUM_Disable = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<ProbeSampleMethod>))]
    public enum ProbeSampleMethod
    {
        UseBounds = 0x0,
        ForceSingle = 0x1,
        ForceMultiple = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<DisplayQuality>))]
    public enum DisplayQuality
    {
        Low = 0x0,
        Medium = 0x1,
        High = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<RadiositySampleRate>))]
    public enum RadiositySampleRate
    {
        Low = 0x0,
        Medium = 0x1,
        High = 0x2,
        VeryHigh = 0x3,
    }

    [MetaSerializer(typeof(EnumSerializer<UpdateMethodWithDefault>))]
    public enum UpdateMethodWithDefault
    {
        Dynamic = 0x0,
        Static = 0x1,
        UseLevelDefault = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<ProbeResolutionWithDefault>))]
    public enum ProbeResolutionWithDefault
    {
        Full = 0x0,
        _1_2 = 0x1,
        _1_4 = 0x2,
        _1_8 = 0x3,
        UseLevelDefault = 0x4,
    }

    [MetaSerializer(typeof(EnumSerializer<AgentUsage>))]
    public enum AgentUsage
    {
        Default = 0x0,
        Aggressive = 0x1,
        Conservative = 0x2,
    }

    [MetaSerializer(typeof(EnumSerializer<SimplifyMode>))]
    public enum SimplifyMode
    {
        None = 0x0,
        SimplifyNoUvs = 0x1,
        SimplifyUsingUvs = 0x2,
    }
}

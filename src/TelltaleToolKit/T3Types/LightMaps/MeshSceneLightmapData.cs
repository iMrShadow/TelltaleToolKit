using System.Numerics;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.LightMaps;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<MeshSceneLightmapData>))]
public class MeshSceneLightmapData
{
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Entry>))]
    public class Entry
    {
        [MetaMember("mMeshName")]
        public Symbol MeshName{ get; set; }

        [MetaMember("mLODIndex")]
        public uint LODIndex{ get; set; }

        [MetaMember("mLightQuality")]
        public int LightQuality{ get; set; }

        [MetaMember("mTextureScale")]
        public Vector2 TextureScale{ get; set; }

        [MetaMember("mTextureOffset")]
        public Vector2 TextureOffset{ get; set; }

        [MetaMember("mTexturePage")]
        public uint TexturePage{ get; set; }
    }

    [MetaMember("mEntries")]
    public List<Entry> Entries { get; set; }

    [MetaMember("mStationaryLightIndices")]
    public List<ushort> StationaryLightIndices{ get; set; }
    [MetaMember("mFlags")]
    public Flags Flags{ get; set; }
    // Explicit mapping for the camera-facing type member requested.
    [MetaMember("mCameraFacingType")]
    public CameraFacingType CameraFacingType{ get; set; }
}

// Example enum for camera-facing type. Replace/extend as appropriate.
[MetaClassSerializerGlobal(typeof(EnumSerializer<CameraFacingType>))]
public enum CameraFacingType
{
    None = 0,
    Billboard = 1,
    Cylindrical = 2,
    Spherical = 3
}
using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.LightMaps;

[MetaSerializer(typeof(MetaClassSerializer<MeshSceneLightmapData>))]
public class MeshSceneLightmapData
{
    [MetaSerializer(typeof(MetaClassSerializer<Entry>))]
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
[MetaSerializer(typeof(EnumSerializer<CameraFacingType>))]
public enum CameraFacingType
{
    None = 0,
    Billboard = 1,
    Cylindrical = 2,
    Spherical = 3
}

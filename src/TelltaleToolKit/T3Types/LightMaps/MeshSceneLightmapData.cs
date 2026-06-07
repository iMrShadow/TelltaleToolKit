using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.LightMaps;

[MetaSerializer(typeof(MetaClassSerializer<MeshSceneLightmapData>))]
public class MeshSceneLightmapData
{
    [MetaMember("mEntries")]
    public List<Entry> Entries { get; set; } = [];

    [MetaMember("mStationaryLightIndices")]
    public List<ushort> StationaryLightIndices { get; set; } = [];

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mCameraFacingType")]
    public CameraFacingType CameraFacingType { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<Entry>))]
    public class Entry
    {
        [MetaMember("mMeshName")]
        public Symbol MeshName { get; set; } = Symbol.Empty;

        [MetaMember("mLODIndex")]
        public uint LODIndex { get; set; }

        [MetaMember("mLightQuality")]
        public int LightQuality { get; set; }

        [MetaMember("mTextureScale")]
        public Vector2 TextureScale { get; set; }

        [MetaMember("mTextureOffset")]
        public Vector2 TextureOffset { get; set; }

        [MetaMember("mTexturePage")]
        public uint TexturePage { get; set; }
    }
}

[MetaSerializer(typeof(EnumSerializer<CameraFacingType>))]
public enum CameraFacingType
{
    None = 0,
    Billboard = 1,
    Cylindrical = 2,
    Spherical = 3
}

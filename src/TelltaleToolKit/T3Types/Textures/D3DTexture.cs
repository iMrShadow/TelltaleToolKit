namespace TelltaleToolKit.T3Types.Textures;

// TODO: Merge with T3Texture
// [DataSerializerGlobal(typeof(D3DTextureClassSerializer))]
// public class D3DTexture
// {
//     [MetaMember("mName")] public string Name { get; set; } = string.Empty;
//
//     [MetaMember("mImportName")] public string ImportName { get; set; } = string.Empty;
//     [MetaMember("mbHasTextureData")] public bool HasTextureData { get; set; } = false;
//
//     [MetaMember("mbIsMipMapped")] public bool IsMipMapped { get; set; } = false;
//
//     [MetaMember("mbIsWrapU")] public bool IsWrapU { get; set; } = false;
//
//     [MetaMember("mbIsWrapV")] public bool IsWrapV { get; set; } = false;
//
//     [MetaMember("mbIsFiltered")] public bool IsFiltered { get; set; } = false;
//
//     [MetaMember("mNumMipLevels")] public uint NumMipLevels { get; set; } = 0;
//
//     [MetaMember("mD3DFormat")] public uint D3DFormat { get; set; } = 0;
//
//     [MetaMember("mWidth")] public uint Width { get; set; } = 0;
//
//     [MetaMember("mHeight")] public uint Height { get; set; } = 0;
//
//     [MetaMember("mType")] public int Type { get; set; } = 0;
//
//     [MetaMember("mbAlphaHDR")] public bool AlphaHdr { get; set; } = false;
//
//     [MetaMember("mbEncrypted")] public bool IsEncrypted { get; set; } = false;
//
//     [MetaMember("mbUsedAsBumpmap")] public bool UsedAsBumpmap { get; set; } = false;
//
//     [MetaMember("mbUsedAsDetailMap")] public bool UsedAsDetailMap { get; set; } = false;
//
//     [MetaMember("mDetailMapBrightness")] public float DetailMapBrightness { get; set; }
//
//     public byte[] DdsTextureData { get; set; } = [];
//
//     public class D3DTextureClassSerializer : MetaClassSerializer<D3DTexture>
//     {
//         public override void Serialize(ref D3DTexture obj, MetaStream stream, MetaClass desc)
//         {
//             // Default Serializer
//             new DefaultClassSerializer<D3DTexture>().Serialize(ref obj, stream, desc);
//
//             if (stream is MetaStreamWriter streamWriter)
//             {
//                 streamWriter.Write(obj.DdsTextureData.Length);
//                 streamWriter.Write(obj.DdsTextureData);
//             }
//             else if (stream is MetaStreamReader streamReader)
//             {
//                 if (!obj.HasTextureData)
//                 {
//                     return;
//                 }
//
//                 //    T3MetaStream.SerializeByteArray(ref texture.DdsTextureData, stream);
//                 var bufferSize = streamReader.ReadInt32();
//                 obj.DdsTextureData = streamReader.ReadBytes(bufferSize);
//             }
//         }
//     }
// }
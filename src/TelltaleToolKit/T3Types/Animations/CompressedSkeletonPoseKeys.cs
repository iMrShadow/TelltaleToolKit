using System.Runtime.InteropServices;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class CompressedSkeletonPoseKeys : IAnimatedValueInterface
{
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public struct Header
    {
        public Vector4 MinDeltaV { get; set; }
        public Vector4 RangeDeltaV { get; set; }
        public Vector4 MinDeltaQ { get; set; }
        public Vector4 RangeDeltaQ { get; set; }
        public Vector4 MinVector { get; set; }
        public Vector4 RangeVector { get; set; }
        public int BoneCount { get; set; }
        public int SampleCount { get; set; }
        public int ValueCount { get; set; }
        public int BlockCount { get; set; }
    }

    public Header GetHeader()
    {
        int size = Marshal.SizeOf<Header>();
        if (Data.Length < size)
            throw new ArgumentException("Data too small for header");

        GCHandle handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
        try
        {
            var header = Marshal.PtrToStructure<Header>(
                handle.AddrOfPinnedObject());
            return header;
        }
        finally
        {
            handle.Free();
        }
    }

    [MetaMember("mDataSize")]
    public int DataSize { get; set; }

    public byte[] Data { get; set; }

    public class Serializer : MetaClassSerializer<CompressedSkeletonPoseKeys2>
    {
        public override void PreSerialize(ref CompressedSkeletonPoseKeys2 obj, MetaStream stream,
            MetaClassType? type = null)
        {
            obj ??= new CompressedSkeletonPoseKeys2();
        }

        public override void Serialize(ref CompressedSkeletonPoseKeys2 obj, MetaStream stream)
        {
            // TODO: Test this type.
            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // TODO: Try experimenting with Marshall.
                obj.DataSize = streamReader.ReadInt32(); // this is the size of struct

                obj.Data = streamReader.ReadBytes(obj.DataSize);
            }
        }
    }
}
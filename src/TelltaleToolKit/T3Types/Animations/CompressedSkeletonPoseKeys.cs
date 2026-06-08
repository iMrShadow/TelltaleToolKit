using System.Numerics;
using System.Runtime.InteropServices;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class CompressedSkeletonPoseKeys : IAnimationValueInterface
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

    public class Serializer : MetaSerializer<CompressedSkeletonPoseKeys>
    {
        public override void PreSerialize(ref CompressedSkeletonPoseKeys? obj, MetaStream stream,
            MetaClassType? type = null)
        {
            obj ??= new CompressedSkeletonPoseKeys();
        }

        public override void Serialize(ref CompressedSkeletonPoseKeys obj, MetaStream stream, MetaClassType? type = null)
        {
            // TODO: Test this type.
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                // TODO: Try experimenting with Marshall.
                obj.DataSize = stream.ReadInt32(); // this is the size of struct

                if (obj.DataSize > 0x1000000)
                {
                    throw new InvalidOperationException("CompressedSkeletonPoseKeys data size is too big.");
                }

                obj.Data = stream.ReadBytes(obj.DataSize);
            }
        }
    }
}

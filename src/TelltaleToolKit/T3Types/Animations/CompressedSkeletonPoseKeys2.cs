using System.Numerics;
using System.Runtime.InteropServices;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class CompressedSkeletonPoseKeys2 : IAnimationValueInterface
{
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public struct Header
    {
        public Vector3 MinDeltaV { get; set; }
        public Vector3 RangeDeltaV { get; set; }
        public Vector3 MinDeltaQ { get; set; }
        public Vector3 RangeDeltaQ { get; set; }
        public Vector3 MinVector { get; set; }
        public Vector3 RangeVector { get; set; }
        public float RangeTime { get; set; }
        public uint BoneCount { get; set; }
        public ulong SampleDataSize { get; set; }
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

    public class Serializer : MetaSerializer<CompressedSkeletonPoseKeys2>
    {
        public override void PreSerialize(ref CompressedSkeletonPoseKeys2? obj, MetaStream stream,
            MetaClassType? type = null)
        {
            obj ??= new CompressedSkeletonPoseKeys2();
        }

        public override void Serialize(ref CompressedSkeletonPoseKeys2 obj, MetaStream stream, MetaClassType? type = null)
        {
            // TODO: Test this type.
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                // TODO: Try experimenting with Marshall.
                obj.DataSize = stream.ReadInt32(); // this is the size of struct

                obj.Data = stream.ReadBytes(obj.DataSize);
            }
        }
    }
}

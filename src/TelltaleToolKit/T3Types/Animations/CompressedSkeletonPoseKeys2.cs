using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class CompressedSkeletonPoseKeys2 : IAnimatedValueInterface
{
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public Vector3 MinDeltaV { get; set; }
    public Vector3 RangeDeltaV { get; set; }
    public Vector3 MinDeltaQ { get; set; }
    public Vector3 RangeDeltaQ { get; set; }
    public Vector3 MinVector { get; set; }
    public Vector3 RangeVector { get; set; }
    public float RangeTime { get; set; }
    public uint BoneCount { get; set; }
    public ulong SampleDataSize { get; set; }

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

                obj.MinDeltaV = new Vector3()
                    { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };
                obj.RangeDeltaV = new Vector3()
                    { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };
                obj.MinDeltaQ = new Vector3()
                    { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };
                obj.RangeDeltaQ = new Vector3()
                    { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };
                obj.MinVector = new Vector3()
                    { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };
                obj.RangeVector = new Vector3()
                    { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };

                obj.RangeTime = streamReader.ReadSingle();
                obj.BoneCount = streamReader.ReadUInt32();
                obj.SampleDataSize = streamReader.ReadUInt64();

                obj.Data = streamReader.ReadBytes(obj.DataSize - 88);
            }
        }
    }
}
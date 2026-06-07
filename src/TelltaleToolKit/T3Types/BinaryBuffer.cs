using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(Serializer))]
public class BinaryBuffer
{
    public byte[] Data { get; set; } = [];

    public class Serializer : MetaSerializer<BinaryBuffer>
    {
        public override void PreSerialize(ref BinaryBuffer? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new BinaryBuffer();

        public override void Serialize(ref BinaryBuffer obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Data.Length);
                stream.Write(obj.Data);
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                int bufferSize = stream.ReadInt32();
                obj.Data = stream.ReadBytes(bufferSize);
            }
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class BinaryBuffer
{
    public class Serializer : MetaClassSerializer<BinaryBuffer>
    {
        public override void PreSerialize(ref BinaryBuffer obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new BinaryBuffer();
        }

        public override void Serialize(ref BinaryBuffer obj, MetaStream stream)
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

    public byte[] Data { get; set; }
}

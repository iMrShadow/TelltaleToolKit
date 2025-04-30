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
            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.Data.Length);
                streamWriter.Write(obj.Data);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int bufferSize = streamReader.ReadInt32();
                obj.Data = streamReader.ReadBytes(bufferSize);
            }
        }
    }

    public byte[] Data { get; set; }
}
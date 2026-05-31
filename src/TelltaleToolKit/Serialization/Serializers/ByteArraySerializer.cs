using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(ByteArraySerializer))]
public class ByteArraySerializer : MetaClassSerializer<byte[]>
{
    public override void Serialize(ref byte[] obj, MetaStream stream)
    {
        if (stream is BinaryMetaStreamWriter streamWriter)
            streamWriter.Write(obj);
        else if (stream is BinaryMetaStreamReader streamReader)
        {
            int size = streamReader.ReadInt32();
            obj = streamReader.ReadBytes(size);
        }
    }
}

using System.Numerics;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(Vector2Serializer))]
public class Vector2Serializer : MetaClassSerializer<Vector2>
{
    public override void Serialize(ref Vector2 obj, MetaStream stream)
    {
        if (stream is BinaryMetaStreamWriter streamWriter)
        {
            if (streamWriter.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Vector2));
                streamWriter.AddVersionInfo(description);
            }

            streamWriter.Write(obj.X);
            streamWriter.Write(obj.Y);
        }
        else if (stream is BinaryMetaStreamReader streamReader)
        {
            obj.X = streamReader.ReadSingle();
            obj.Y = streamReader.ReadSingle();
        }
    }
}

[MetaClassSerializerGlobal(typeof(Vector3Serializer))]
public class Vector3Serializer : MetaClassSerializer<Vector3>
{
    public override void Serialize(ref Vector3 obj, MetaStream stream)
    {
        if (stream is BinaryMetaStreamWriter streamWriter)
        {
            if (streamWriter.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Vector3));
                streamWriter.AddVersionInfo(description);
            }

            streamWriter.Write(obj.X);
            streamWriter.Write(obj.Y);
            streamWriter.Write(obj.Z);
        }
        else if (stream is BinaryMetaStreamReader streamReader)
        {
            obj.X = streamReader.ReadSingle();
            obj.Y = streamReader.ReadSingle();
            obj.Z = streamReader.ReadSingle();
        }
    }
}

[MetaClassSerializerGlobal(typeof(Vector4Serializer))]
public class Vector4Serializer : MetaClassSerializer<Vector4>
{
    public override void Serialize(ref Vector4 obj, MetaStream stream)
    {
        if (stream is BinaryMetaStreamWriter streamWriter)
        {
            if (streamWriter.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Vector4));
                streamWriter.AddVersionInfo(description);
            }

            streamWriter.Write(obj.X);
            streamWriter.Write(obj.Y);
            streamWriter.Write(obj.Z);
            streamWriter.Write(obj.W);
        }
        else if (stream is BinaryMetaStreamReader streamReader)
        {
            obj.X = streamReader.ReadSingle();
            obj.Y = streamReader.ReadSingle();
            obj.Z = streamReader.ReadSingle();
            obj.W = streamReader.ReadSingle();
        }
    }
}

[MetaClassSerializerGlobal(typeof(QuaternionSerializer))]
public class QuaternionSerializer : MetaClassSerializer<Quaternion>
{
    public override void Serialize(ref Quaternion obj, MetaStream stream)
    {
        if (stream is BinaryMetaStreamWriter streamWriter)
        {
            if (streamWriter.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Quaternion));
                streamWriter.AddVersionInfo(description);
            }

            streamWriter.Write(obj.X);
            streamWriter.Write(obj.Y);
            streamWriter.Write(obj.Z);
            streamWriter.Write(obj.W);
        }
        else if (stream is BinaryMetaStreamReader streamReader)
        {
            obj.X = streamReader.ReadSingle();
            obj.Y = streamReader.ReadSingle();
            obj.Z = streamReader.ReadSingle();
            obj.W = streamReader.ReadSingle();
        }
    }
}
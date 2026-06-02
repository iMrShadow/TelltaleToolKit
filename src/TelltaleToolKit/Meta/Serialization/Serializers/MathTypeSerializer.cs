using System.Numerics;
using TelltaleToolKit.Meta.Reflection;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(Vector2Serializer))]
public class Vector2Serializer : MetaClassSerializer<Vector2>
{
    public override void Serialize(ref Vector2 obj, MetaStream stream)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            if (stream.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Vector2));
                stream.AddVersionInfo(description);
            }

            stream.Write(obj.X);
            stream.Write(obj.Y);
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            obj.X = stream.ReadSingle();
            obj.Y = stream.ReadSingle();
        }
    }
}

[MetaClassSerializerGlobal(typeof(Vector3Serializer))]
public class Vector3Serializer : MetaClassSerializer<Vector3>
{
    public override void Serialize(ref Vector3 obj, MetaStream stream)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            if (stream.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Vector3));
                stream.AddVersionInfo(description);
            }

            stream.Write(obj.X);
            stream.Write(obj.Y);
            stream.Write(obj.Z);
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            obj.X = stream.ReadSingle();
            obj.Y = stream.ReadSingle();
            obj.Z = stream.ReadSingle();
        }
    }
}

[MetaClassSerializerGlobal(typeof(Vector4Serializer))]
public class Vector4Serializer : MetaClassSerializer<Vector4>
{
    public override void Serialize(ref Vector4 obj, MetaStream stream)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            if (stream.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Vector4));
                stream.AddVersionInfo(description);
            }

            stream.Write(obj.X);
            stream.Write(obj.Y);
            stream.Write(obj.Z);
            stream.Write(obj.W);
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            obj.X = stream.ReadSingle();
            obj.Y = stream.ReadSingle();
            obj.Z = stream.ReadSingle();
            obj.W = stream.ReadSingle();
        }
    }
}

[MetaClassSerializerGlobal(typeof(QuaternionSerializer))]
public class QuaternionSerializer : MetaClassSerializer<Quaternion>
{
    public override void Serialize(ref Quaternion obj, MetaStream stream)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            if (stream.Params.CanModifySerializedClassesList)
            {
                MetaClass? description = stream.GetMetaClass(typeof(Quaternion));
                stream.AddVersionInfo(description);
            }

            stream.Write(obj.X);
            stream.Write(obj.Y);
            stream.Write(obj.Z);
            stream.Write(obj.W);
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            obj.X = stream.ReadSingle();
            obj.Y = stream.ReadSingle();
            obj.Z = stream.ReadSingle();
            obj.W = stream.ReadSingle();
        }
    }
}

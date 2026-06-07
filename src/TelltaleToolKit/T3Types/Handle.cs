using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types;

public class HandleObjectInfo
{
    public Flags Flags;
    public object HandleObject;
    public Symbol ObjectName = Symbol.Empty;
    public MetaClassType Type;

    public override string ToString() => $"Symbol: {ObjectName}";
}

[MetaSerializer(typeof(Serializer))]
public class HandleBase
{
    public HandleObjectInfo ObjectInfo = new();

    public override string ToString() => $"Handle: {ObjectInfo}";

    public class Serializer : MetaSerializer<HandleBase>
    {
        public override void PreSerialize(ref HandleBase? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new HandleBase();

        public override void Serialize(ref HandleBase obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Params.StreamVersion >= 5)
            {
                stream.Serialize(ref obj.ObjectInfo.ObjectName);
            }
            else
            {
                string name = string.Empty;
                stream.Serialize(ref name);
                obj.ObjectInfo.ObjectName = Symbol.FromName(name);
            }
        }
    }
}

[MetaSerializer(typeof(HandleSerializer<>), typeof(Handle<>))]
public class Handle<T> : HandleBase
{
}

[MetaSerializer(typeof(HandleSerializer<>), typeof(HandleLock<>))]
public class HandleLock<T> : Handle<T>
{
}

public class HandleSerializer<T> : MetaSerializer<Handle<T>>
{
    public override void PreSerialize(ref Handle<T>? obj, MetaStream stream, MetaClassType? type = null) =>
        obj ??= new Handle<T>();

    public override void Serialize(ref Handle<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        HandleBase handle = obj;
        stream.Serialize(ref handle);
    }
}

public class HandleLockSerializer<T> : MetaSerializer<HandleLock<T>>
{
    public override void PreSerialize(ref HandleLock<T>? obj, MetaStream stream, MetaClassType? type = null) =>
        obj ??= new HandleLock<T>();

    public override void Serialize(ref HandleLock<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        HandleBase handle = obj;
        stream.Serialize(ref handle);
    }
}

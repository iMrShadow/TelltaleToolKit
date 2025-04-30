using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(BoolSerializer))]
public sealed class BoolSerializer : MetaClassSerializer<bool>
{
    /// <inheritdoc/>
    public override void Serialize(ref bool obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(SByteSerializer))]
public sealed class SByteSerializer : MetaClassSerializer<sbyte>
{
    /// <inheritdoc/>
    public override void Serialize(ref sbyte obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(ByteSerializer))]
public sealed class ByteSerializer : MetaClassSerializer<byte>
{
    /// <inheritdoc/>
    public override void Serialize(ref byte obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(Int16ClassSerializer))]
public sealed class Int16ClassSerializer : MetaClassSerializer<short>
{
    /// <inheritdoc/>
    public override void Serialize(ref short obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(UInt16ClassSerializer))]
public sealed class UInt16ClassSerializer : MetaClassSerializer<ushort>
{
    /// <inheritdoc/>
    public override void Serialize(ref ushort obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(Int32ClassSerializer))]
public sealed class Int32ClassSerializer : MetaClassSerializer<int>
{
    /// <inheritdoc/>
    public override void Serialize(ref int obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(UInt32ClassSerializer))]
public sealed class UInt32ClassSerializer : MetaClassSerializer<uint>
{
    /// <inheritdoc/>
    public override void Serialize(ref uint obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(Int64ClassSerializer))]
public sealed class Int64ClassSerializer : MetaClassSerializer<long>
{
    /// <inheritdoc/>
    public override void Serialize(ref long obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(UInt64ClassSerializer))]
public sealed class UInt64ClassSerializer : MetaClassSerializer<ulong>
{
    /// <inheritdoc/>
    public override void Serialize(ref ulong obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(SingleClassSerializer))]
public sealed class SingleClassSerializer : MetaClassSerializer<float>
{
    /// <inheritdoc/>
    public override void Serialize(ref float obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(DoubleClassSerializer))]
public sealed class DoubleClassSerializer : MetaClassSerializer<double>
{
    /// <inheritdoc/>
    public override void Serialize(ref double obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}

[MetaClassSerializerGlobal(typeof(StringClassSerializer))]
public sealed class StringClassSerializer : MetaClassSerializer<string>
{
    /// <inheritdoc/>
    public override void Serialize(ref string obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref string obj, MetaStream stream, MetaClassType? type)
    {
    }
}

public sealed class EnumSerializer<TEnum> : MetaClassSerializer<TEnum> where TEnum : Enum
{
    /// <inheritdoc/>
    public override void Serialize(ref TEnum obj, MetaStream stream)
    {
        if (stream is MetaStreamWriter streamWriter)
        {
            streamWriter.Write(Convert.ToInt32(obj));
        }
        else if (stream is MetaStreamReader streamReader)
        {
            obj = (TEnum)Enum.ToObject(typeof(TEnum), streamReader.ReadInt32());
        }
    }
}
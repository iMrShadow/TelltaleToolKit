using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(SymbolSerializer))]
public class SymbolSerializer : MetaSerializer<Symbol>
{
    public override void PreSerialize(ref Symbol? obj, MetaStream stream, MetaClassType? type = null)
    {
        obj ??= Symbol.Empty;
    }

    public override void Serialize(ref Symbol obj, MetaStream stream)
    {
        MetaClass? description = stream.GetMetaClass(typeof(Symbol));

        if (description is null)
        {
            throw new InvalidOperationException("Symbol description not found.");
        }

        if (stream.Mode == MetaStreamMode.Write)
        {
            stream.AddVersionInfo(description);
        }

        stream.Serialize(ref obj);
    }
}

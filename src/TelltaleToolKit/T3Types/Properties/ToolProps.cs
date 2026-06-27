using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Properties;

[MetaSerializer(typeof(Serializer))]
public class ToolProps
{
    [MetaMember("mbHasProps")]
    public bool HasProps { get; set; }

    public PropertySet Properties { get; set; } = new();

    public class Serializer : MetaSerializer<ToolProps>
    {
        public override void PreSerialize(ref ToolProps? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new ToolProps();
        }

        public override void Serialize(ref ToolProps obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                // Quoting Lucas: "we are not in the tool engine, runtime engine. would normally be a #ifdef COMPILING_AS_TOOL etc"
                obj.HasProps = false;
                stream.Write(obj.HasProps);
            }
            else
            {
                obj.HasProps = stream.ReadBoolean();

                if (!obj.HasProps)
                    return;

                var properties = new PropertySet();
                stream.Serialize(ref properties);
                obj.Properties = properties;
            }
        }
    }
}

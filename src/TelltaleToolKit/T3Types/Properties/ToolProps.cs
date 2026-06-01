using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Properties;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class ToolProps
{
    [MetaMember("mbHasProps")]
    public bool HasProps { get; set; }

    public class Serializer : MetaClassSerializer<ToolProps>
    {
        public override void PreSerialize(ref ToolProps obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new ToolProps();
            }
        }

        public override void Serialize(ref ToolProps obj, MetaStream stream)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                // Quoting Lucas: "we are not in the tool engine, runtime engine. would normally be a #ifdef COMPILING_AS_TOOL etc"
                obj.HasProps = false;
                stream.Write(obj.HasProps);
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                obj.HasProps = stream.ReadBoolean();

                if (!obj.HasProps)
                    return;

                var runtimeProperties = new PropertySet();
                Toolkit.Instance.GetSerializer<PropertySet>().Serialize(ref runtimeProperties, stream);
                throw new InvalidDataException("This type has tool properties. Please report this to the author!");
            }
        }
    }
}

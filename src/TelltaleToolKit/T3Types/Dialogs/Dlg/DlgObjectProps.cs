using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DlgObjectProps
{
    public enum PropsType {
        UserProps = 1,
        ProductionProps = 2,
        ToolProps = 4
    }
    
    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
    
    public PropertySet UserProperties { get; set; }
    public PropertySet ProductionProperties { get; set; }
    public PropertySet ToolProperties { get; set; }
    public class Serializer : MetaClassSerializer<DlgObjectProps>
    {
        private static readonly DefaultClassSerializer<DlgObjectProps> DefaultSerializer = new();

        public override void Serialize(ref DlgObjectProps obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if ((obj.Flags.Data & (int)PropsType.UserProps) != 0)
                {
                    var userProps = new PropertySet();
                    TTK.PreSerialize(ref userProps, stream);
                    TTK.Serialize(ref userProps, stream);
                    obj.UserProperties = userProps;
                }
                
                if ((obj.Flags.Data & (int)PropsType.ProductionProps) != 0)
                {
                    var prodProps = new PropertySet();
                    TTK.PreSerialize(ref prodProps, stream);
                    TTK.Serialize(ref prodProps, stream);
                    obj.ProductionProperties = prodProps;
                }
                
                if ((obj.Flags.Data & (int)PropsType.ToolProps) != 0)
                {
                    var toolProps = new PropertySet();
                    TTK.PreSerialize(ref toolProps, stream);
                    TTK.Serialize(ref toolProps, stream);
                    obj.ToolProperties = toolProps;
                }
            }
        }
    }
}
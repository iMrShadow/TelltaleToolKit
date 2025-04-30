using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Properties;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class PropertySet
{
    // Starts appearing later
    [MetaMember("mPropVersion")]
    public int PropVersion { get; set; }

    [MetaMember("mPropertyFlags")]
    public Flags PropertyFlags { get; set; } = new();

    // Disappears
    [MetaMember("mParentList")]
    public List<Handle<PropertySet>> ParentList { get; set; } = [];

    public Dictionary<Symbol, object> Properties = [];
    public PropertySet ParentProperties;

    public class Serializer : MetaClassSerializer<PropertySet>
    {
        private static readonly DefaultClassSerializer<PropertySet> DefaultSerializer = new();

        public override void PreSerialize(ref PropertySet obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new PropertySet();
            }
        }

        public override void Serialize(ref PropertySet obj, MetaStream stream)
        {
            DefaultSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                if (obj.PropVersion > 0)
                {
                    obj.ParentList.Capacity = streamReader.ReadInt32();

                    for (var i = 0; i < obj.ParentList.Capacity; i++)
                    {
                        var parent = new Handle<PropertySet>();
                        TTKContext.Instance().GetSerializer<Handle<PropertySet>>().Serialize(ref parent, stream);
                        obj.ParentList.Add(parent);
                    }
                }

                if (obj.PropVersion == 1)
                {
                    stream.EndBlock();
                    stream.BeginBlock();
                }

                int numTypes = streamReader.ReadInt32();
                for (var i = 0; i < numTypes; i++)
                {
                    // The type of the class 
                    MetaClassType typeSymbol = streamReader.ReadMetaClassType();

                    // The number of times that type has been serialized
                    int numOfType = streamReader.ReadInt32();

                    MetaClassSerializer classTypeSerializer =
                        TTKContext.Instance().GetSerializer(typeSymbol.LinkingType);

                    for (var j = 0; j < numOfType; j++)
                    {
                        // The property key
                        Symbol propertyKey = streamReader.ReadSymbol();

                        object? propertyValue = null;

                        classTypeSerializer.PreSerialize(ref propertyValue, stream, typeSymbol);
                        classTypeSerializer.Serialize(ref propertyValue, stream);
                        obj.Properties[propertyKey] = propertyValue;
                    }
                }

                bool embeddedParentProps = (obj.PropertyFlags.Data & 1024) != 0;

                // If the flag is true
                if (embeddedParentProps)
                {
                    TTKContext.Instance().GetSerializer<PropertySet>().Serialize(ref obj.ParentProperties, stream);
                }
            }

            stream.EndBlock();
        }
    }
}
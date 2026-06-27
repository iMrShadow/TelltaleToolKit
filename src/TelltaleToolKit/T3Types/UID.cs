using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Common
{
    namespace UID
    {
        // TODO: Convert these into interfaces.
        [MetaSerializer(typeof(MetaClassSerializer<Generator>))]
        public class Generator
        {
            [MetaMember("miNextUniqueID")]
            public int NextUniqueId { get; set; }
        }

        public interface IGenerator
        {
            public Generator Generator { get; set; }
        }

        [MetaSerializer(typeof(MetaClassSerializer<Owner>))]
        public class Owner
        {
            [MetaMember("miUniqueID")]
            public int UniqueId { get; set; }
        }

        public interface IOwner
        {
            public Owner Owner { get; set; }
        }
    }

    [MetaSerializer(typeof(Serializer))]
    public class ActingOverridablePropOwner
    {
        // I prefer chicken.
        public const uint kHeader = 0xBEEFF00D;

        [MetaMember("mSerializationFlags")]
        public Flags SerializationFlags { get; set; }

        [MetaMember("mpOverridableValues")]
        public PropertySet? OverridableValues { get; set; } = new();

        [MetaMember("mhParent")]
        public Handle<PropertySet> Parent { get; set; } = new();

        public uint Header { get; set; }

        public class Serializer : MetaSerializer<ActingOverridablePropOwner>
        {
            private static readonly MetaClassSerializer<ActingOverridablePropOwner> s_metaClassSerializer = new();

            public override void PreSerialize(ref ActingOverridablePropOwner? obj, MetaStream stream,
                MetaClassType? type = null)
            {
                obj ??= new ActingOverridablePropOwner();
            }

            public override void Serialize(ref ActingOverridablePropOwner obj, MetaStream stream,
                MetaClassType? type = null)
            {
                if (stream.Mode is MetaStreamMode.Write)
                {
                    bool hasValues = obj.OverridableValues is { Properties.Count: > 0 };
                    if (hasValues)
                        obj.SerializationFlags.Clear(1); // clear bit 0
                    else
                        obj.SerializationFlags.Set(1);

                    if (obj.Header == kHeader)
                    {
                        stream.Write(kHeader);
                        s_metaClassSerializer.PreSerialize(ref obj!, stream);
                        s_metaClassSerializer.Serialize(ref obj, stream);
                    }

                    if (hasValues)
                    {
                        PropertySet? propertySet = obj.OverridableValues;
                        stream.Serialize(ref propertySet);
                    }
                }
                else
                {
                    long startPos = stream.GetPosition();
                    uint header = stream.ReadUInt32();

                    if (header == kHeader)
                    {
                        s_metaClassSerializer.PreSerialize(ref obj!, stream);
                        s_metaClassSerializer.Serialize(ref obj, stream);

                        if ((obj.SerializationFlags.Data & 1) == 0) // has values
                        {
                            PropertySet propertySet = new();
                            stream.Serialize(ref propertySet);
                            obj.OverridableValues = propertySet;
                        }
                        else
                        {
                            obj.OverridableValues = null; // or empty PropertySet, as needed
                        }
                    }
                    else
                    {
                        stream.SetPosition(startPos);
                        PropertySet propertySet = new();
                        stream.Serialize(ref propertySet);
                        obj.OverridableValues = propertySet;
                    }
                }
            }
        }
        // TODO: Add helper functions
    }
}

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


    public class ActingOverridablePropOwner
    {
        // I prefer chicken.
        public const uint kHeader = 0xBEEFF00D;

        [MetaMember("mSerializationFlags")]
        public Flags SerializationFlags { get; set; }

        [MetaMember("mpOverridableValues")]
        public PropertySet OverridableValues { get; set; } = new();

        [MetaMember("mhParent")]
        public Handle<PropertySet> Parent { get; set; } = new();

        // TODO: Add helper functions
        // TODO: Add a proper serializer
    }

    [MetaSerializer(typeof(ActingOverridablePropOwnerSerializer))]
    public class ActingOverridablePropOwnerSerializer : MetaSerializer<ActingOverridablePropOwner>
    {
        private static readonly MetaClassSerializer<ActingOverridablePropOwner> s_metaClassSerializer = new();

        public override void PreSerialize(ref ActingOverridablePropOwner? obj, MetaStream stream,
            MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new ActingOverridablePropOwner();
            }
        }

        public override void Serialize(ref ActingOverridablePropOwner obj, MetaStream stream)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                long currPos = stream.GetPosition();
                uint value = stream.ReadUInt32();

                if (value == ActingOverridablePropOwner.kHeader)
                {
                    s_metaClassSerializer.PreSerialize(ref obj, stream);
                    s_metaClassSerializer.Serialize(ref obj, stream);
                    return;
                }

                stream.SetPosition(currPos);
                if ((obj.SerializationFlags.Data & 1) == 0)
                    return;

                PropertySet propertySet = obj.OverridableValues;
                stream.Serialize(ref propertySet);
            }
        }
    }
}

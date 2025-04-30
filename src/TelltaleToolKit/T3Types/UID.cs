using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Common
{
    namespace UID
    {
        // TODO: Convert these into interfaces.
        [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Generator>))]
        public class Generator
        {
            [MetaMember("miNextUniqueID")]
            public int NextUniqueId { get; set; }
        }
        public interface IGenerator
        {
            public Generator Generator { get; set; }
        }

        [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Owner>))]
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

    [MetaClassSerializerGlobal(typeof(ActingOverridablePropOwnerSerializer))]
    public class ActingOverridablePropOwnerSerializer : MetaClassSerializer<ActingOverridablePropOwner>
    {
        private static readonly DefaultClassSerializer<ActingOverridablePropOwner> DefaultSerializer = new();

        public override void PreSerialize(ref ActingOverridablePropOwner obj, MetaStream stream,
            MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new ActingOverridablePropOwner();
            }
        }

        public override void Serialize(ref ActingOverridablePropOwner obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                long currPos = streamReader.GetCurrentPosition();
                uint value = streamReader.ReadUInt32();

                if (value == ActingOverridablePropOwner.kHeader)
                {
                    DefaultSerializer.PreSerialize(ref obj, stream);
                    DefaultSerializer.Serialize(ref obj, stream);
                    return;
                }

                streamReader.SetCurrentPosition(currPos);
                if ((obj.SerializationFlags.Data & 1) == 0)
                    return;

                PropertySet propertySet = obj.OverridableValues;
                TTKContext.Instance().GetSerializer<PropertySet>().PreSerialize(ref propertySet, stream);
                TTKContext.Instance().GetSerializer<PropertySet>().Serialize(ref propertySet, stream);
            }
        }
    }
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Animations;

// TODO: Add Interface
[MetaClassSerializerGlobal(typeof(Serializer))]
public class Animation
{
    [MetaMember("mVersion")]
    public int Version { get; set; }

    // This is a string and a symbol at the same time
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mLength")]
    public float Length { get; set; } = 0.0f;

    [MetaMember("mAdditiveMask")]
    public float AdditiveMask { get; set; } = 0.0f;

    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();

    [MetaMember("mToolProps")]
    public ToolProps ToolProps { get; set; } = new();

    // This is internal
    public class InterfaceInfo
    {
        public MetaClassType Type { get; set; }
        public int ValueCount { get; set; }
        public uint Version { get; set; }
    }


    public List<InterfaceInfo> Descriptors { get; set; } = [];

    public List<IAnimatedValueInterface> Values { get; set; } = [];

    public byte[] Buffer = [];

    public class Serializer : MetaClassSerializer<Animation>
    {
        private static readonly DefaultClassSerializer<Animation> DefaultSerializer = new();

        public override void Serialize(ref Animation obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            // Go to the serializer for old animations
            if (obj.Version <= 0)
            {
                SerializeOldAnimation(ref obj, stream);
                return;
            }

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int numTotalValues = streamReader.ReadInt32();
                obj.Values = new List<IAnimatedValueInterface>(numTotalValues);

                // Runtime buffer (not needed for now)
                int dataBufferSize = streamReader.ReadInt32();
                obj.Buffer = new byte[dataBufferSize];
                
                int numValueTypes = streamReader.ReadInt32();
                obj.Descriptors = new List<InterfaceInfo>(numValueTypes);

                for (var i = 0; i < numValueTypes; i++)
                {
                    MetaClassType typeSymbol = streamReader.ReadMetaClassType(); // The type of the class

                    int numOfType = streamReader.ReadInt16(); // The number of times that type has been serialized
                    // TODO: Verify what these versions actually represent.
                    // TelltaleToolLib casts to unsigned int and uses it as a CRC32.
                    var version = (uint)streamReader.ReadInt16();

                    var interfaceInfo = new InterfaceInfo
                    {
                        Type = typeSymbol,
                        Version = version,
                        ValueCount = numOfType
                    };
                    obj.Descriptors.Add(interfaceInfo);
                }

                foreach (InterfaceInfo desc in obj.Descriptors)
                {
                    MetaClassSerializer serializer = TTKContext.Instance().GetSerializer(desc.Type.LinkingType);

                    for (var j = 0; j < desc.ValueCount; j++)
                    {
                        object? propertyValue = null;
                        
                        serializer.PreSerialize(ref propertyValue, stream);
                        serializer.Serialize(ref propertyValue, stream);

                        if (propertyValue is IAnimatedValueInterface value)
                            obj.Values.Add(value);
                    }
                }
                
                // These are very weird hacks by Telltale. What an...interesting system
                // If for some reason the serializer for the baseclass of type `AnimationValueInterfaceBase` is not called,
                // we read its values - the Symbol (CRC64 in this case) and the flags.
                foreach (IAnimatedValueInterface value in obj.Values)
                {
                    // TODO: Set other flags?
                    value.AnimationValueInterfaceBase.Flags = streamReader.ReadInt32();
                }

                ushort isNotInterface = streamReader.ReadUInt16();

                if (isNotInterface == 0)
                {
                    foreach (IAnimatedValueInterface value in obj.Values)
                    {
                        value.AnimationValueInterfaceBase.Name = streamReader.ReadSymbol();
                    }
                }
            }

            stream.EndBlock();
        }


        public void SerializeOldAnimation(ref Animation obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int numTotalValues = streamReader.ReadInt32();
                int numValueTypes = streamReader.ReadInt32();

                for (var i = 0; i < numValueTypes; i++)
                {
                    MetaClassType typeSymbol = streamReader.ReadMetaClassType(); // The type of the class
                    int numOfType = streamReader.ReadInt32(); // The number of times that type has been serialized

                    MetaClassSerializer serializer =
                        TTKContext.Instance().GetSerializer(typeSymbol.LinkingType);

                    for (var j = 0; j < numOfType; j++)
                    {
                        object? propertyValue = null;

                        serializer.PreSerialize(ref propertyValue, stream);
                        serializer.Serialize(ref propertyValue, stream);
                        numTotalValues++;
                    }
                }
            }
            
            stream.EndBlock();
        }
    }
}
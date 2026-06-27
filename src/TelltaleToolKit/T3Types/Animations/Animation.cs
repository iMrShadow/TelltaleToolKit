using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Animations;

// TODO: Add Interface
[MetaSerializer(typeof(Serializer))]
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

    public List<IAnimationValueInterface> Values { get; set; } = [];

    public byte[] Buffer = [];

    public class Serializer : MetaSerializer<Animation>
    {
        private static readonly MetaClassSerializer<Animation> s_metaClassSerializer = new();

        public override void Serialize(ref Animation obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            // Go to the serializer for old animations
            if (obj.Version <= 0)
            {
                SerializeOldAnimation(ref obj, stream);
                return;
            }

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int numTotalValues = stream.ReadInt32();
                obj.Values = new List<IAnimationValueInterface>(numTotalValues);

                // Runtime buffer (not needed for now)
                int dataBufferSize = stream.ReadInt32();
                obj.Buffer = new byte[dataBufferSize];

                int numValueTypes = stream.ReadInt32();
                obj.Descriptors = new List<InterfaceInfo>(numValueTypes);

                for (var i = 0; i < numValueTypes; i++)
                {
                    MetaClassType? typeSymbol = stream.ReadMetaClassType(); // The type of the class
                    if (typeSymbol is null)
                    {
                        // Graceful crash if the type is not registered.
                        // This is extremely rare, only a handful of files have been identified to have unregistered types.
                        // I assume those are leftovers from the editor, but somehow got into the main game.
                        // Also, it's possible that the asset is not used at all.
                        stream.EndBlock();
                        return;
                    }

                    int numOfType = stream.ReadInt16(); // The number of times that type has been serialized
                    // TODO: Verify what these versions actually represent.
                    // TelltaleToolLib casts to unsigned int and uses it as a CRC32.
                    var version = (uint)stream.ReadInt16();

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
                    MetaSerializer serializer = Toolkit.Instance.GetSerializer(desc.Type.LinkingType);

                    for (var j = 0; j < desc.ValueCount; j++)
                    {
                        object? propertyValue = null;

                        serializer.PreSerialize(ref propertyValue, stream, desc.Type);
                        serializer.Serialize(ref propertyValue, stream, desc.Type);

                        if (propertyValue is IAnimationValueInterface value)
                        {

                            obj.Values.Add(value);
                        }
                        else
                        {
                            throw new InvalidOperationException($"[Animation] Expected {nameof(IAnimationValueInterface)} but got {propertyValue?.GetType().Name ?? "null"}");
                        }
                    }
                }

                // These are very weird hacks by Telltale. What an...interesting system
                // If for some reason the serializer for the baseclass of type `AnimationValueInterfaceBase` is not called,
                // we read its values - the Symbol (CRC64 in this case) and the flags.
                foreach (IAnimationValueInterface value in obj.Values)
                {
                    // TODO: Set other flags?
                    value.AnimationValueInterfaceBase.Flags = stream.ReadInt32();
                }

                ushort isNotInterface = stream.ReadUInt16();

                if (isNotInterface == 0)
                {
                    // TODO: Symbol vs String
                    foreach (IAnimationValueInterface value in obj.Values)
                    {
                        value.AnimationValueInterfaceBase.NameS = stream.ReadSymbol();
                    }
                }
            }

            stream.EndBlock();
        }

        public void SerializeOldAnimation(ref Animation obj, MetaStream stream)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int numTotalValues = stream.ReadInt32();
                int numValueTypes = stream.ReadInt32();

                for (var i = 0; i < numValueTypes; i++)
                {
                    MetaClassType? typeSymbol = stream.ReadMetaClassType(); // The type of the class
                    if (typeSymbol is null)
                        throw new InvalidOperationException("[Animation] Type symbol is not registered.");

                    int numOfType = stream.ReadInt32(); // The number of times that type has been serialized

                    MetaSerializer serializer = Toolkit.Instance.GetSerializer(typeSymbol.LinkingType);

                    for (var j = 0; j < numOfType; j++)
                    {
                        object? propertyValue = null;

                        serializer.PreSerialize(ref propertyValue, stream, typeSymbol);
                        serializer.Serialize(ref propertyValue, stream, typeSymbol);
                        numTotalValues++;
                    }
                }
            }

            stream.EndBlock();
        }
    }
}

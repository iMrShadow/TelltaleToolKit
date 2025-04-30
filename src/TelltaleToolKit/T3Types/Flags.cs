using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Flags>))]
public struct Flags
{
    // TODO: Potentially migrate to C# enum flags.
    // Some issues arise with the serializing process. For e.g., determining how the `class Flags` should be added to the serialized classes list.
    // Also, it will make serializers slower (but not by much). The reason why is because the type's property search complexity goes to O(N) instead of O(1).
    
    [MetaMember("mFlags")] public int Data { get; set; }

    public override string ToString()
    {
        return $"{Data}";
    }
}
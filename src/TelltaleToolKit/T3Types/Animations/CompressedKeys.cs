using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(CompressedKeys<>.Serializer), typeof(CompressedKeys<>))]
public class CompressedKeys<T> : IAnimationValueInterface
{
    [MetaMember("Baseclass_AnimationValueInterfaceBase")]
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public List<T> Keys { get; set; } = [];
    public List<float> Times { get; set; } = [];
    public byte[] Tangents = [];

    public class Serializer : MetaSerializer<CompressedKeys<T>>
    {
        public override void PreSerialize(ref CompressedKeys<T>? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new CompressedKeys<T>();
        }

        public override void Serialize(ref CompressedKeys<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write((short)obj.Keys.Count);

                for (short i = 0; i < obj.Keys.Count; i++)
                {
                    T objKey = obj.Keys[i];
                    stream.Serialize(ref objKey);
                    stream.Write(obj.Times[i]);
                }

                stream.Write(obj.Tangents);
            }
            else
            {
                int numKeys = stream.ReadInt16();
                obj.Keys = new List<T>(numKeys);

                for (int i = 0; i < numKeys; i++)
                {
                    var key = default(T);
                    stream.Serialize(ref key);
                    obj.Keys.Add(key);

                    var time = stream.ReadSingle();
                    obj.Times.Add(time);
                }

                obj.Tangents = stream.ReadBytes((numKeys + 3) / 4);
            }
        }
    }
}

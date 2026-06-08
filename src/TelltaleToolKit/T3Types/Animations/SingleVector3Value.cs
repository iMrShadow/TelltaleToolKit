using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class SingleVector3Value : IAnimationValueInterface
{
    public Vector3 DecompressedValue { get; set; }

    public class Serializer : MetaSerializer<SingleVector3Value>
    {
        public override void PreSerialize(ref SingleVector3Value? obj, MetaStream stream,
            MetaClassType? type = null) =>
            obj ??= new SingleVector3Value();

        public override void Serialize(ref SingleVector3Value obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else
            {
                int compressedValue = stream.ReadInt32();

                if (compressedValue == -1)
                {
                    Vector3 vector3 = new Vector3();
                    stream.Serialize(ref vector3);
                    obj.DecompressedValue = vector3;
                }
                else
                {
                    obj.DecompressedValue = DecompressSingleVector3(compressedValue);
                }
            }
        }

        private static readonly float[] _maxBounds = { 1.0f, 1.5f, 2.0f };

        private static Vector3 DecompressSingleVector3(int compressed)
        {
            int idx = (int)(compressed >> 30);
            float max = _maxBounds[idx];
            float min = -max;

            static float DecompressComponent(int bits, float min, float max)
            {
                const int maxVal = (1 << 10) - 1;
                float t = (bits & maxVal) / (float)maxVal;
                return min + t * (max - min);
            }

            float x = DecompressComponent(compressed, min, max);
            float y = DecompressComponent(compressed >> 10, min, max);
            float z = DecompressComponent(compressed >> 20, min, max);
            return new Vector3(x, y, z);
        }
    }

    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();
}

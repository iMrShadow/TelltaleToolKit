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
                uint compressedValue = stream.ReadUInt32();

                if ((int)compressedValue == -1)
                {
                    Vector3 vector3 = new Vector3();
                    stream.Serialize(ref vector3);
                    obj.DecompressedValue = vector3;
                }
                else
                {
                    obj.DecompressedValue = DecompressVector3(compressedValue);
                }
            }
        }

        private static readonly float[] s_maxBounds = { 1.0f, 1.5f, 2.0f };

        /// <summary>
        /// Decompresses a 32‑bit packed value into a Vector3.
        /// </summary>
        /// <param name="packed">The packed uint.</param>
        /// <param name="s_maxBounds">Array of 4 bounds corresponding to the top 2 bits.</param>
        /// <returns>The decompressed vector.</returns>
        public static Vector3 DecompressVector3(uint packed)
        {
            // Extract the 2‑bit index from bits 30‑31.
            int boundIndex = (int)(packed >> 30);
            float bound = s_maxBounds[boundIndex];

            // Extract each 10‑bit component.
            int xVal = (int)(packed & 0x3FF);
            int yVal = (int)((packed >> 10) & 0x3FF);
            int zVal = (int)((packed >> 20) & 0x3FF);

            float x = DecompressComponent(xVal, bound);
            float y = DecompressComponent(yVal, bound);
            float z = DecompressComponent(zVal, bound);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Maps a 10‑bit unsigned integer (0..1023) to a float in the range [-bound, bound].
        /// </summary>
        private static float DecompressComponent(int value, float bound)
        {
            const int maxVal = 1023;           // 2¹⁰ - 1
            // Linear interpolation: -bound + (value / maxVal) * (2 * bound)
            return (value / (float)maxVal) * 2f * bound - bound;
        }
    }

    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();
}

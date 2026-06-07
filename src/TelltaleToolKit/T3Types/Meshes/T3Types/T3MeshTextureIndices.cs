using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(Serializer))]
public class T3MeshTextureIndices
{
    public int[] Index { get; set; } = [-1, -1]; // Usually 2 elements: for batch Default and for batch Shadow

    public class Serializer : MetaSerializer<T3MeshTextureIndices>
    {
        public override void Serialize(ref T3MeshTextureIndices obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj?.Index != null)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        int index = obj.Index[i];
                        if ((index & int.MinValue) == 0) // int.MinValue == 0x80000000
                        {
                            int idx = i;
                            stream.Write(idx);
                            stream.Write(index);
                        }
                    }
                }

                // Write the end
                stream.Write(-1);
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                int i = stream.ReadInt32();

                while ((i & int.MinValue) == 0)
                {
                    int x = stream.ReadInt32();

                    if (i < 2)
                        obj.Index[i] = x;
                    i = stream.ReadInt32();
                }
            }
        }
    }
}

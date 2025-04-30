using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

 [MetaClassSerializerGlobal(typeof(Serializer))]
public class T3MeshTextureIndices
{
    public int[] Index { get; set; } = [-1, -1]; // Usually 2 elements: for batch Default and for batch Shadow

    public class Serializer : MetaClassSerializer<T3MeshTextureIndices>
    {
        public override void Serialize(ref T3MeshTextureIndices obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                for (var i = 0; i < 2; i++)
                {
                    int index = obj.Index[i];
                    if ((index & int.MinValue) == 0) // int.MinValue == 0x80000000
                    {
                        int idx = i;
                        streamWriter.Write(idx);
                        streamWriter.Write(index);
                    }

                    // Write the end
                    streamWriter.Write(-1);
                }
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int i = streamReader.ReadInt32();

                while ((i & int.MinValue) == 0)
                {
                    int x = streamReader.ReadInt32();

                    if (i < 2)
                        obj.Index[i] = x;
                    i = streamReader.ReadInt32();
                }
            }
        }
    }
}
using System.IO;

namespace Stereome
{
    public struct BBoneWeight
    {
        public static UnityEngine.BoneWeight Read(BinaryReader reader)
        {
            return new UnityEngine.BoneWeight
            {
                weight0 = reader.ReadSingle(),
                weight1 = reader.ReadSingle(),
                weight2 = reader.ReadSingle(),
                weight3 = reader.ReadSingle(),
                boneIndex0 = reader.ReadInt32(),
                boneIndex1 = reader.ReadInt32(),
                boneIndex2 = reader.ReadInt32(),
                boneIndex3 = reader.ReadInt32()
            };
        }

        public static UnityEngine.BoneWeight[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            var result = new UnityEngine.BoneWeight[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = Read(reader);
            }
            return result;
        }


        public static void Write(UnityEngine.BoneWeight boneWeight, BinaryWriter writer)
        {
            writer.Write(boneWeight.weight0);
            writer.Write(boneWeight.weight1);
            writer.Write(boneWeight.weight2);
            writer.Write(boneWeight.weight3);
            writer.Write(boneWeight.boneIndex0);
            writer.Write(boneWeight.boneIndex1);
            writer.Write(boneWeight.boneIndex2);
            writer.Write(boneWeight.boneIndex3);
        }

        public static void Write(UnityEngine.BoneWeight[] array, BinaryWriter writer)
        {
            int count = array.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                Write(array[i], writer);
            }
        }
    }
}
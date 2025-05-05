using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public struct BColor32
    {

        // Burst 컴파일러를 사용한 Job 정의
        [BurstCompile]
        private struct ConvertByteToVector4Job : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<UnityEngine.Color32> ColorArray;

            public void Execute(int index)
            {
                int byteIndex = index * 4;
                // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                UnityEngine.Color32 vector = new UnityEngine.Color32(
                    ByteArray[byteIndex],
                    ByteArray[byteIndex + 1],
                    ByteArray[byteIndex + 2],
                    ByteArray[byteIndex + 3]
                );
                ColorArray[index] = vector;
            }
        }

        public static UnityEngine.Color32 Read(BinaryReader reader)
        {
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();
            byte a = reader.ReadByte();
            return new UnityEngine.Color32(r, g, b, a);
        }

        public static UnityEngine.Color32[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * 4), Allocator.TempJob);
            using var vector = new NativeArray<UnityEngine.Color32>(count, Allocator.TempJob);
            var job = new ConvertByteToVector4Job()
            {
                ByteArray = bytes,
                ColorArray = vector
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            return vector.ToArray();
        }


        public static void Write(UnityEngine.Color32 color32, BinaryWriter writer)
        {
            writer.Write(color32.r);
            writer.Write(color32.g);
            writer.Write(color32.b);
            writer.Write(color32.a);
        }

        public static void Write(UnityEngine.Color32[] array, BinaryWriter writer)
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
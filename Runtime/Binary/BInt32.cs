using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public struct BInt32
    {
        // Burst 컴파일러를 사용한 Job 정의
        [BurstCompile]
        private struct ConvertByteToIntJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<int> IntArray;

            public void Execute(int index)
            {
                int byteIndex = index * sizeof(int);
                IntArray[index] = BUnsafe.ReadIntFromBytes(ByteArray, byteIndex);
            }
        }

        public static int[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(int)), Allocator.TempJob);
            using var array = new NativeArray<int>(count, Allocator.TempJob);
            var job = new ConvertByteToIntJob()
            {
                ByteArray = bytes,
                IntArray = array
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            return array.ToArray();
        }

        public static void Write(int[] array, BinaryWriter writer)
        {
            int count = array.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                writer.Write(array[i]);
            }
        }
    }
}
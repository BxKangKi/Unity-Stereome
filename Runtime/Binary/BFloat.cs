using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public struct BFloat
    {
        // Burst 컴파일러를 사용한 Job 정의
        [BurstCompile]
        private struct ConvertByteToFloatJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<float> FloatArray;

            public void Execute(int index)
            {
                int byteIndex = index * sizeof(float);
                FloatArray[index] = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex);
            }
        }

        public static float[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(int)), Allocator.TempJob);
            using var array = new NativeArray<float>(count, Allocator.TempJob);
            var job = new ConvertByteToFloatJob()
            {
                ByteArray = bytes,
                FloatArray = array
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            return array.ToArray();
        }

        public static void Write(float[] array, BinaryWriter writer)
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
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public struct BVector2
    {
        // Burst 컴파일러를 사용한 Job 정의
        [BurstCompile]
        private struct ConvertByteToVector2Job : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<UnityEngine.Vector2> VectorArray;
            public void Execute(int index)
            {
                int byteIndex = index * 2 * sizeof(float);

                // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                UnityEngine.Vector2 vector = new UnityEngine.Vector2(
                    BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex),
                    BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float))
                );
                VectorArray[index] = vector;
            }
        }

        public static UnityEngine.Vector2 Read(BinaryReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            return new UnityEngine.Vector2(x, y);
        }

        public static UnityEngine.Vector2[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 2), Allocator.TempJob);
            using var vector = new NativeArray<UnityEngine.Vector2>(count, Allocator.TempJob);
            var job = new ConvertByteToVector2Job()
            {
                ByteArray = bytes,
                VectorArray = vector
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            return vector.ToArray();
        }

        public static void Write(UnityEngine.Vector2 vector, BinaryWriter writer)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
        }


        public static void Write(UnityEngine.Vector2[] array, BinaryWriter writer)
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
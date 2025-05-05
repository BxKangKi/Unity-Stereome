using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public struct BVector4
    {
        // Burst 컴파일러를 사용한 Job 정의
        [BurstCompile]
        private struct ConvertByteToVector4Job : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<UnityEngine.Vector4> VectorArray;

            public void Execute(int index)
            {
                int byteIndex = index * 4 * sizeof(float);

                // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                UnityEngine.Vector4 vector = new UnityEngine.Vector4(
                    BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex),
                    BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float)),
                    BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 2 * sizeof(float)),
                    BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 3 * sizeof(float))
                );
                VectorArray[index] = vector;
            }
        }

        public static UnityEngine.Vector4 Read(BinaryReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            float w = reader.ReadSingle();
            return new UnityEngine.Vector4(x, y, z, w);
        }

        public static UnityEngine.Vector4[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 4), Allocator.TempJob);
            using var vector = new NativeArray<UnityEngine.Vector4>(count, Allocator.TempJob);
            var job = new ConvertByteToVector4Job()
            {
                ByteArray = bytes,
                VectorArray = vector
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            return vector.ToArray();
        }

        public static void Write(UnityEngine.Vector4 vector, BinaryWriter writer)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
            writer.Write(vector.w);
        }

        public static void Write(UnityEngine.Vector4[] array, BinaryWriter writer)
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
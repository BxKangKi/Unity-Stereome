using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public struct BMatrix4x4
    {
        [BurstCompile]
        private struct ConvertByteToMatrix4x4Job : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<UnityEngine.Matrix4x4> MatrixArray;

            public void Execute(int index)
            {
                int byteIndex = index * 16 * sizeof(float);

                // ByteArray에서 각 요소를 직접 변환하여 Matrix4x4를 만듭니다.
                UnityEngine.Matrix4x4 matrix = new UnityEngine.Matrix4x4
                {
                    m00 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex),
                    m01 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float)),
                    m02 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 2 * sizeof(float)),
                    m03 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 3 * sizeof(float)),
                    m10 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 4 * sizeof(float)),
                    m11 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 5 * sizeof(float)),
                    m12 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 6 * sizeof(float)),
                    m13 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 7 * sizeof(float)),
                    m20 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 8 * sizeof(float)),
                    m21 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 9 * sizeof(float)),
                    m22 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 10 * sizeof(float)),
                    m23 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 11 * sizeof(float)),
                    m30 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 12 * sizeof(float)),
                    m31 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 13 * sizeof(float)),
                    m32 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 14 * sizeof(float)),
                    m33 = BUnsafe.ReadFloatFromBytes(ByteArray, byteIndex + 15 * sizeof(float)),
                };

                MatrixArray[index] = matrix;
            }
        }

        public static UnityEngine.Matrix4x4 Read(BinaryReader reader)
        {
            return new UnityEngine.Matrix4x4
            {
                m00 = reader.ReadSingle(),
                m01 = reader.ReadSingle(),
                m02 = reader.ReadSingle(),
                m03 = reader.ReadSingle(),
                m10 = reader.ReadSingle(),
                m11 = reader.ReadSingle(),
                m12 = reader.ReadSingle(),
                m13 = reader.ReadSingle(),
                m20 = reader.ReadSingle(),
                m21 = reader.ReadSingle(),
                m22 = reader.ReadSingle(),
                m23 = reader.ReadSingle(),
                m30 = reader.ReadSingle(),
                m31 = reader.ReadSingle(),
                m32 = reader.ReadSingle(),
                m33 = reader.ReadSingle()
            };
        }

        public static UnityEngine.Matrix4x4[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * 16 * sizeof(float)), Allocator.TempJob);
            using var matrix = new NativeArray<UnityEngine.Matrix4x4>(count, Allocator.TempJob);
            ConvertByteToMatrix4x4Job job = new ConvertByteToMatrix4x4Job
            {
                ByteArray = bytes,
                MatrixArray = matrix
            };

            JobHandle handle = job.Schedule(count, -1);
            handle.Complete();
            return matrix.ToArray();
        }


        public static void Write(UnityEngine.Matrix4x4 matrix, BinaryWriter writer)
        {
            writer.Write(matrix.m00);
            writer.Write(matrix.m01);
            writer.Write(matrix.m02);
            writer.Write(matrix.m03);
            writer.Write(matrix.m10);
            writer.Write(matrix.m11);
            writer.Write(matrix.m12);
            writer.Write(matrix.m13);
            writer.Write(matrix.m20);
            writer.Write(matrix.m21);
            writer.Write(matrix.m22);
            writer.Write(matrix.m23);
            writer.Write(matrix.m30);
            writer.Write(matrix.m31);
            writer.Write(matrix.m32);
            writer.Write(matrix.m33);
        }

        public static void Write(UnityEngine.Matrix4x4[] array, BinaryWriter writer)
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
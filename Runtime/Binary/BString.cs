using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Stereome
{
    public struct BString
    {
        // Burst 컴파일러를 사용한 Job 정의
        [BurstCompile]
        private struct ConvertByteToFixedString512BytesJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> ByteArray;
            public NativeArray<FixedString512Bytes> StringArray;
            public void Execute(int index)
            {
                int byteIndex = index * 2 * sizeof(float);
                // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                FixedString512Bytes str = BUnsafe.ReadFixedString64BytesFromBytes(ByteArray, byteIndex);
                StringArray[index] = str;
            }
        }

        public static string[] ReadArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 2), Allocator.TempJob);
            using var str = new NativeArray<FixedString512Bytes>(count, Allocator.TempJob);
            var job = new ConvertByteToFixedString512BytesJob()
            {
                ByteArray = bytes,
                StringArray = str
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            string[] array = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                array[i] = str[i].ToString();
            }
            return array;
        }

        public static void Write(string str, BinaryWriter writer)
        {
            var fixedstr = new FixedString512Bytes(str);
            int size = UnsafeUtility.SizeOf<FixedString512Bytes>();
            NativeArray<byte> bytes = new NativeArray<byte>(size, Allocator.Temp);
            unsafe
            {
                void* ptr = UnsafeUtility.AddressOf(ref fixedstr);
                UnsafeUtility.MemCpy(bytes.GetUnsafePtr(), ptr, size);
            }
            writer.Write(bytes);
        }


        public static void Write(string[] array, BinaryWriter writer)
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
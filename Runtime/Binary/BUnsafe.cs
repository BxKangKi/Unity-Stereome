using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Stereome
{
    internal struct BUnsafe
    {
        public static FixedString64Bytes ReadFixedString64BytesFromBytes(NativeArray<byte> array, int startIndex)
        {
            // Burst-friendly 방식으로 포인터 참조
            unsafe
            {
                byte* bytePtr = (byte*)array.GetUnsafeReadOnlyPtr() + startIndex;
                return *(FixedString64Bytes*)bytePtr;
            }
        }

        public static float ReadFloatFromBytes(NativeArray<byte> array, int startIndex)
        {
            // Burst-friendly 방식으로 포인터 참조
            unsafe
            {
                byte* bytePtr = (byte*)array.GetUnsafeReadOnlyPtr() + startIndex;
                return *(float*)bytePtr;
            }
        }

        public static int ReadIntFromBytes(NativeArray<byte> array, int startIndex)
        {
            // Burst-friendly 방식으로 포인터 참조
            unsafe
            {
                byte* bytePtr = (byte*)array.GetUnsafeReadOnlyPtr() + startIndex;
                return *(int*)bytePtr;
            }
        }
    }
}
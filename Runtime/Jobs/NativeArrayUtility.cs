using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Stereome
{
    public static class NativeArrayUtility
    {
        /// <summary>
        /// Make NativeArray<T> each elements has same T value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static NativeArray<T> ToNativeArray<T>(T value, int count) where T : unmanaged
        {
            var result = new NativeArray<T>(count, Allocator.TempJob);
            var job = new ToNativeArrayJob<T>()
            {
                result = result,
                value = value
            };
            var handle = job.Schedule(count, -1);
            handle.Complete();
            return result;
        }


        // ToNativeArray IJobParallelFor struct with burst compile.
        [BurstCompile]
        private struct ToNativeArrayJob<T> : IJobParallelFor where T : unmanaged
        {
            [WriteOnly] public NativeArray<T> result;
            [ReadOnly] public T value;
            public void Execute(int i)
            {
                result[i] = value;
            }
        }
    }

}
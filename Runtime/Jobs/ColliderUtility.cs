using UnityEngine;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;

namespace Stereome
{
    public struct ColliderUtility
    {
        public static int NearestFromCollider(Collider[] colls, float3 origin, out float3 outPos)
        {
            int count = colls.Length;
            using var position = new NativeArray<float4>(GetPositionArrayFromColliders(colls), Allocator.TempJob);
            using var distance = new NativeArray<float>(count, Allocator.TempJob);
            var job = new DistanceJob()
            {
                position = position,
                distance = distance,
                origin = origin
            };
            JobHandle handle = job.Schedule(count, -1);
            handle.Complete();
            int index = FindShortest(distance);
            outPos = position[index].xyz;
            return index;
        }

        public static int NearestFromCollider(Collider[] colls, float3 origin)
        {
            return NearestFromCollider(colls, origin, out var _);
        }

        private static int FindShortest(NativeArray<float> distance)
        {
            float shortest = distance[0];
            int index = 0;
            for (int i = 0; i < distance.Length; i++)
            {
                if (shortest > distance[i])
                {
                    shortest = distance[i];
                    index = i;
                }
            }
            return index;
        }

        private static float4[] GetPositionArrayFromColliders(Collider[] colls)
        {
            float4[] results = new float4[colls.Length];
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != null)
                {
                    results[i] = new float4(colls[i].gameObject.transform.position, 1f);
                }
                else
                {
                    results[i] = float4.zero;
                }
            }
            return results;
        }


        [BurstCompile]
        public struct DistanceJob : IJobParallelFor
        {
            [WriteOnly] public NativeArray<float> distance;
            [ReadOnly] public NativeArray<float4> position;
            [ReadOnly] public float3 origin;
            public void Execute(int i)
            {
                if (position[i].w >= 0.5f)
                {
                    distance[i] = math.distance(origin, position[i].xyz);
                }
                else
                {
                    distance[i] = float.MaxValue;
                }
            }
        }
    }
}
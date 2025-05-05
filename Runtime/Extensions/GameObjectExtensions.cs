using UnityEngine;

namespace Stereome
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Check GameObject has component, unless add component. after that, return component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T AccessComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent<T>(out var component))
            {
                return go.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Check GameObject has component, unless Add component. After that, return component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns>If GameObject was had component, return false, or return true</returns>
        public static bool TryAddComponent<T>(this GameObject go, out T component) where T : Component
        {
            if (!go.TryGetComponent<T>(out component))
            {
                component = go.AddComponent<T>();
                return true;
            }
            return false;
        }

        public static void InitTransform(this GameObject go, Transform parent = null)
        {
            go.transform.SetParent(parent);
            go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            go.transform.localScale = Vector3.one;
        }

        public static Matrix4x4 WorldTRS(this GameObject go)
        {
            go.transform.GetPositionAndRotation(out var pos, out var rot);
            return Matrix4x4.TRS(pos, rot, go.transform.lossyScale);
        }

        public static Matrix4x4 LocalTRS(this GameObject go)
        {
            go.transform.GetLocalPositionAndRotation(out var pos, out var rot);
            return Matrix4x4.TRS(pos, rot, go.transform.localScale);
        }
    }
}
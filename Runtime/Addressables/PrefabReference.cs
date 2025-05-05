#if ADDRESSABLES
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Stereome
{
    /// <summary>
    /// Inherit UnityEngine.AddressableAssets.AssetReference class. Specialized for GameObject AssetReference.
    /// </summary>
    [Serializable]
    public class PrefabReference : AssetReference
    {
        protected GameObject gameObject;
        public GameObject GameObject { get => gameObject; }

        /// <summary>
        /// Check gameObject is exist currently.
        /// </summary>
        public bool IsLoaded { get => gameObject != null; }

        /// <summary>
        /// Set Active PrefabReference's gameObject.
        /// </summary>
        /// <param name="value"></param>
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }


        /// <summary>
        /// Destroy gameObject when it exist
        /// </summary>
        public void Destroy()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }

        /// <summary>
        /// InstantiateAsync for PrefabReference.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerator InstantiateAsync(MonoBehaviour behaviour, Transform parent = null)
        {
            yield return behaviour.StartCoroutine(InstantiateAsync_Internal(parent, Vector3.zero, Quaternion.identity));
        }

        /// <summary>
        /// InstantiateAsync for PrefabReference.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerator InstantiateAsync(MonoBehaviour behaviour, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            yield return behaviour.StartCoroutine(InstantiateAsync_Internal(parent, position, rotation));
        }

        // InstantiateAsync for PrefabReference.
        private IEnumerator InstantiateAsync_Internal(Transform parent, Vector3 position, Quaternion rotation)
        {
            if (gameObject == null)
            {
                var handle = base.InstantiateAsync(position, rotation, parent);
                yield return handle;
                gameObject = handle.Result;
                gameObject.AddComponent<ReleaseDestroy>();
            }
        }

        /// <summary>
        /// Wait until gameObject is exist.
        /// </summary>
        /// <returns></returns>
        public IEnumerator CheckGameObjectAsync()
        {
            yield return new WaitUntil(() => gameObject != null);
        }
    }
}
#endif
#if ADDRESSABLES
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Stereome
{
    public static class AssetReferenceExtensions
    {
        /// <summary>
        /// InstantiateAsync coroutine.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerator InstantiateAsync(this AssetReference asset, MonoBehaviour behaviour, Transform parent = null)
        {
            yield return behaviour.StartCoroutine(InstantiateAsync_Internal(asset, parent, Vector3.zero, Quaternion.identity));
        }


        /// <summary>
        /// InstantiateAsync coroutine.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerator InstantiateAsync(this AssetReference asset, MonoBehaviour behaviour, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            yield return behaviour.StartCoroutine(InstantiateAsync_Internal(asset, parent, pos, rot));
        }

        private static IEnumerator InstantiateAsync_Internal(AssetReference asset, Transform parent, Vector3 pos, Quaternion rot)
        {
            var handle = asset.InstantiateAsync(pos, rot, parent);
            yield return handle;
            handle.Result.AddComponent<ReleaseDestroy>();
        }

        /// <summary>
        /// LoadSceneAsync coroutine.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IEnumerator LoadSceneAsync(this AssetReference asset, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var handle = asset.LoadSceneAsync(mode, false);
            yield return handle;
            yield return handle.Result.ActivateAsync();
        }
    }
}
#endif
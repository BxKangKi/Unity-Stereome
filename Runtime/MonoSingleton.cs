using UnityEngine;

namespace Stereome
{
    /// <summary>
    /// MonoBehaviour singleton pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Private instance vairable.
        private static T instance;

        // Public instance variable getter.
        public static T Instance
        {
            get
            {
                // Check current singleton is null (not exist)
                if (instance == null)
                {
                    // If singleton is not exist, assgin object from FindAnyObjectByType() method.
                    instance = (T)FindAnyObjectByType(typeof(T));
#if UNITY_EDITOR
                    // Print error to console if instance is not exist.
                    if (instance == null)
                    {
                        Debug.Log(string.Format("Singleton '{0}' not found. null returned.", typeof(T)));
                    }
#endif
                }
                return instance;
            }
        }
    }
}

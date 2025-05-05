using UnityEngine;

namespace Stereome
{
    /// <summary>
    /// ScriptableObject singleton pattern. ScriptableObject asset should be located in 'Resources' directory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] asset = Resources.LoadAll<T>("");
                    if (asset == null || asset.Length < 1 || asset.Length > 1)
                    {
                        Debug.Log($"Asset is not exist or more than one in 'Resources' directory");
                    }
                    instance = asset[0];
                }

                return instance;
            }
        }
    }
}
using System.Text;
using UnityEngine;

namespace Stereome
{
    /// <summary>
    /// Serialization between byte[](string) and object.
    /// </summary>
    public readonly struct Serialization
    {
        /// <summary>
        /// Deserialize object from json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Deserialize object to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="prettyPrint"></param>
        /// <returns></returns>
        public static string Serialize<T>(T data, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(data, prettyPrint);
        }
    }
}
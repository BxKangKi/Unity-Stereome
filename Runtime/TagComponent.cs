using UnityEngine;

namespace Stereome
{
    /// <summary>
    /// Custom tag component. Attach to gameObject.
    /// </summary>
    public class TagComponent : MonoBehaviour
    {
        public string[] tags;

        public virtual bool CheckTagEqual(string s)
        {
            return System.Array.Exists(tags, element => element == s);
        }

        /// <summary>
        /// Add tag string.
        /// </summary>
        /// <param name="s"></param>
        public void Add(string s)
        {
            if (tags == null)
            {
                tags = new string[] { s };
            }
            else
            {
                tags.Add(s);
            }
        }

        /// <summary>
        /// Remove tag string.
        /// </summary>
        /// <param name="s"></param>
        public void Remove(string s)
        {
            if (tags != null)
            {
                tags.Remove(s);
            }
        }
    }
}

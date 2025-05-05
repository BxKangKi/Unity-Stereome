using UnityEngine;

namespace Stereome
{
    /// <summary>
    /// When gameObject awake, it destroy self.
    /// </summary>
    public class SelfDestroy : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}
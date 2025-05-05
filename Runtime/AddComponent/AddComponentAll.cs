using UnityEngine;

namespace Stereome
{
    public class AddComponentAll : MonoBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Add(transform.GetChild(i).gameObject);
            }
            Destroy(this);
        }

        protected virtual void Add(GameObject go) { }
    }
}
using UnityEngine;

namespace Stereome
{
    public class AddRendererToLODGroup : MonoBehaviour
    {
        public float cullingPoint = 0.01f;
        public bool animate = false;

        private void Start()
        {
            var mesh2LOD = new RendererToLODGroup()
            {
                gameObject = gameObject,
                cullingPoint = cullingPoint,
                animate = animate
            };
            mesh2LOD.Start();
            Destroy(this);
        }
    }
}
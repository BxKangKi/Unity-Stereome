using UnityEngine;

namespace Stereome
{
    public class AddRendererToLODGroupAll : AddComponentAll
    {
        public float cullingPoint = 0.01f;
        public bool animate = false;

        protected override void Add(GameObject go)
        {
            var mesh2LOD = new RendererToLODGroup()
            {
                gameObject = go,
                cullingPoint = cullingPoint,
                animate = animate
            };
            mesh2LOD.Start();
        }
    }
}
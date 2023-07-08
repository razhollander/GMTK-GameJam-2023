using System.Linq;
using UnityEngine;

namespace Planet
{
    public class LandPositionGetter : MonoBehaviour
    {
        private MeshFilter _meshFilter;

        public int Vertex
        {

            get
            {
                if (_meshFilter == null)
                {
                    _meshFilter = GetComponent<MeshFilter>();
                }
                return _meshFilter.mesh.vertexCount;
            }
        }

        public Vector3 Position
        {
            get
            {
                var vertices = _meshFilter.mesh.vertices;
                var center = vertices.Aggregate((f, s) => f + s);
                center.x /= Vertex;
                center.y /= Vertex;
                center.z /= Vertex;
                return center;
            }
        }

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
    }
}